using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.EF;
namespace Exam.API.Services
{

    public class BomService : IBomService
    {
        private readonly IBomRepository _repo;
        private readonly ILogger<BomService> _logger;

        public BomService(IBomRepository repo, ILogger<BomService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        
        public async Task<string> CreateAsync(CreateBomDto dto, ClaimsPrincipal user)
        {
            try
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
                _logger.LogInformation($"ProductId:{dto.ProductId} MaterialId:{dto.MaterialId} Bom新增成功");
                return "建立成功";
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"ProductId:{dto.ProductId} MaterialId:{dto.MaterialId} Bom刪除成功");
                throw;
            }
        }

       
        public async Task<string> UpdateAsync(UpdateBomDto dto, ClaimsPrincipal user)
        {
            try
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
                _logger.LogInformation($"MaterialId:{dto.MaterialId} Bom更改成功");
                return "更新成功";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"MaterialId:{dto.MaterialId} Bom更改失敗");
                throw;
            }

            
        }

        
        public async Task<string> DeleteAsync(int bomId)
        {
            try
            {
                var bom = await _repo.GetByIdAsync(bomId);
                if (bom == null)
                    return "BOM不存在";

                // 檢查產品是否被下單
                var hasOrder = await _repo.ProductHasOrderAsync(bom.ProductId);
                if (hasOrder)
                    return "此產品已被下單，BOM不可刪除";

                await _repo.DeleteAsync(bom);
                _logger.LogInformation($"{bomId} Bom刪除成功");
                return "刪除成功";

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{bomId} Bom刪除失敗");
                throw;
            }
        }

        public async Task<IPagedList<BomDto>> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            try
            {
                var query = _repo.GetQuery();

                var pagedData = await query
                    .OrderBy(x => x.BomId)
                    .Select(x => new BomDto
                    {
                        BomId = x.BomId,
                        ProductId = x.ProductId,
                        ProductName = x.Product.ProductName,
                        MaterialId = x.MaterialId,
                        MaterialName = x.Material.MaterialName,
                        Quantity = (int)x.Quantity
                    })
                    .ToPagedListAsync(pageNumber, pageSize);
                _logger.LogInformation("Bom查詢成功");
                return pagedData;

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Bom查詢失敗");
                throw;
            }
        }
    }
}
