using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using System.Security.Claims;
using X.PagedList;
namespace Exam.API.Services
{

    public class BomService : IBomService
    {
        private readonly IBomRepository _repo;

        public BomService(IBomRepository repo)
        {
            _repo = repo;
        }

        
        public async Task<string> CreateAsync(CreateBomDto dto, ClaimsPrincipal user)
        {
            if (string.IsNullOrEmpty(dto.ProductId) || string.IsNullOrEmpty(dto.MaterialId))
                return "產品或物料不可為空";

            if (dto.Quantity <= 0)
                return "數量需大於0";

            if (await _repo.ExistsAsync(dto.ProductId, dto.MaterialId))
                return "此BOM已存在";

            var bom = new Bom
            {
                ProductId = dto.ProductId,
                MaterialId = dto.MaterialId,
                Quantity = dto.Quantity,
                CreatedBy = user.FindFirst(ClaimTypes.Name)?.Value
            };

            await _repo.AddAsync(bom);

            return "建立成功";
        }

       
        public async Task<string> UpdateAsync(UpdateBomDto dto, ClaimsPrincipal user)
        {
            var bom = await _repo.GetByIdAsync(dto.BomId);
            if (bom == null)
                return "BOM不存在";

            if (dto.Quantity <= 0)
                return "數量需大於0";

            // 即使產品被下單  仍允許修改

            bom.MaterialId = dto.MaterialId;
            bom.Quantity = dto.Quantity;
            bom.UpdatedDate = DateTime.Now;
            bom.UpdatedBy = user.FindFirst(ClaimTypes.Name)?.Value;

            await _repo.UpdateAsync(bom);

            return "更新成功";
        }

        
        public async Task<string> DeleteAsync(int bomId)
        {
            var bom = await _repo.GetByIdAsync(bomId);
            if (bom == null)
                return "BOM不存在";

            // 檢查產品是否被下單
            var hasOrder = await _repo.ProductHasOrderAsync(bom.ProductId);
            if (hasOrder)
                return "此產品已被下單，BOM不可刪除";

            await _repo.DeleteAsync(bom);

            return "刪除成功";
        }

        public async Task<IPagedList<BomDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var data = await _repo.GetPagedAsync(pageNumber, pageSize);

            var dtoList = data.Select(x => new BomDto
            {
                BomId = x.BomId,
                ProductId = x.ProductId,
                ProductName = x.Product.ProductName,
                MaterialId = x.MaterialId,
                MaterialName = x.Material.MaterialName,
                Quantity = (int)x.Quantity
            }).ToList();

            return new StaticPagedList<BomDto>(
                dtoList,
                data.PageNumber,
                data.PageSize,
                data.TotalItemCount
            );
        }
    }
}
