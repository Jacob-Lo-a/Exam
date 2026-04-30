using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using System.Security.Claims;
using X.PagedList;
namespace Exam.API.Services
{

    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _repo;

        public MaterialService(IMaterialRepository repo)
        {
            _repo = repo;
        }

       
        public async Task<string> CreateAsync(CreateMaterialDto dto, ClaimsPrincipal user)
        {
            if (dto.Cost < 0)
                return "成本不可小於0";

            var maxId = await _repo.GetMaxMaterialIdAsync();
            var newId = GenerateMaterialId(maxId);

            var material = new Material
            {
                MaterialId = newId,
                MaterialName = dto.MaterialName,
                Cost = dto.Cost,
                Stock = dto.Stock,
                CreatedBy = user.FindFirst(ClaimTypes.Name)?.Value
            };

            await _repo.AddAsync(material);

            return "建立成功";
        }

        
        public async Task<string> UpdateAsync(
            UpdateMaterialDto dto,
            ClaimsPrincipal user)
        {
            var material = await _repo.GetByIdAsync(dto.MaterialId);
            if (material == null)
                return "物料不存在";

            if (dto.Cost < 0)
                return "成本不可小於0";

            if (dto.Stock < 0)
                return "庫存不可小於0";

            material.MaterialName = dto.MaterialName;
            material.Cost = dto.Cost;
            material.Stock = dto.Stock;
            material.UpdatedDate = DateTime.Now;
            material.UpdatedBy = user.FindFirst(ClaimTypes.Name)?.Value;

            await _repo.UpdateAsync(material);

            return "更新成功";
        }

       
        public async Task<string> DeleteAsync(string materialId)
        {
            var material = await _repo.GetByIdAsync(materialId);
            if (material == null)
                return "物料不存在";

           
            var used = await _repo.ExistsInBomAsync(materialId);
            if (used)
                return "此物料已被BOM使用，無法刪除";

            await _repo.DeleteAsync(material);

            return "刪除成功";
        }

        private string GenerateMaterialId(string? maxId)
        {
            if (string.IsNullOrEmpty(maxId))
                return "ML00000001";

            var number = int.Parse(maxId.Substring(2));
            return "ML" + (number + 1).ToString("D8");
        }

        public async Task<IPagedList<MaterialDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var data = await _repo.GetPagedAsync(pageNumber, pageSize);

            
            var result = data.Select(x => new MaterialDto
            {
                MaterialId = x.MaterialId,
                MaterialName = x.MaterialName,
                Cost = x.Cost,
                Stock = (int)x.Stock,
            }).ToList();

            // X.PagedList 
            return new StaticPagedList<MaterialDto>(
                result,
                data.PageNumber,
                data.PageSize,
                data.TotalItemCount
                
            );
        }
    }
}
