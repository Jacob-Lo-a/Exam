using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.EF;
namespace Exam.API.Services
{

    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _repo;
        private readonly ILogger<MaterialService> _logger;
        public MaterialService(IMaterialRepository repo, ILogger<MaterialService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

       
        public async Task<string> CreateAsync(CreateMaterialDto dto, ClaimsPrincipal user)
        {
            try
            {
                if (dto.Cost < 0)
                {
                    return "成本不可小於0";
                }

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
                _logger.LogInformation($"{dto.MaterialName} 物料新增成功");

                return "建立成功";
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"{dto.MaterialName} 物料新增失敗");
                throw;
            }

            
        }

        
        public async Task<string> UpdateAsync(
            UpdateMaterialDto dto,
            ClaimsPrincipal user)
        {
            try
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
                _logger.LogInformation($"{dto.MaterialName} 物料更改成功");
                return "更新成功";
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, $"{dto.MaterialName} 物料更改失敗");
                throw;
            }

            
        }

       
        public async Task<string> DeleteAsync(string materialId)
        {
            try
            {

                var material = await _repo.GetByIdAsync(materialId);
                if (material == null)
                    return "物料不存在";

           
                var used = await _repo.ExistsInBomAsync(materialId);
                if (used)
                    return "此物料已被BOM使用，無法刪除";

                await _repo.DeleteAsync(material);
                _logger.LogInformation($"{materialId} 物料刪除成功");

                return "刪除成功";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{materialId} 物料刪除失敗");
                throw;
            }

            
        }

        private string GenerateMaterialId(string? maxId)
        {
            if (string.IsNullOrEmpty(maxId))
                return "ML00000001";

            var number = int.Parse(maxId.Substring(2));
            return "ML" + (number + 1).ToString("D8");
        }

        public async Task<IPagedList<MaterialDto>> GetPagedAsync(
            int pageNumber,
            int pageSize)
        {
            try
            {
                var query = _repo.GetQuery();

                var pagedData = await query
                    .OrderBy(x => x.MaterialId)
                    .Select(x => new MaterialDto
                    {
                        MaterialId = x.MaterialId,
                        MaterialName = x.MaterialName,
                        Cost = x.Cost,
                        Stock = (int)x.Stock
                    })
                    .ToPagedListAsync(pageNumber, pageSize);

                _logger.LogInformation("物料查詢成功");

                return pagedData;

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "物料查詢失敗");
                throw;
            }
             
            
                
        }
    }
}
