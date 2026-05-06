using Exam.API.Controllers;
using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using System.Data;
using System.Security.Claims;
using X.PagedList;
using X.PagedList.EF;
namespace Exam.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IProductRepository repo, ILogger<ProductService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<string> CreateProductAsync(
            CreateProductDto dto,
            ClaimsPrincipal user)
        {
            try
            {

                if (dto.Price < 0)
                {
                    return "價格不可小於0";
                }

                if (await _repo.ExistsAsync(dto.ProductName))
                {
                    return "產品名稱已存在";
                }

                var maxId = await _repo.GetMaxProductIdAsync();
                var newId = GenerateProductId(maxId);


                var product = new Product
                {
                    ProductId = newId,
                    ProductName = dto.ProductName,
                    Price = dto.Price,
                    CreatedBy = user.FindFirst(ClaimTypes.Name)?.Value
                };

                await _repo.AddAsync(product);
                _logger.LogInformation("產品新增成功");
                return "建立成功";
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "產品新增失敗");
                throw;
            }

            
            
        }

        private string GenerateProductId(string? maxId)
        {
            if (string.IsNullOrEmpty(maxId))
                return "PT00000001";

            var number = int.Parse(maxId.Substring(2));
            return "PT" + (number + 1).ToString("D8");
        }

        public async Task<string> UpdateProductAsync(
            UpdateProductDto dto,
            ClaimsPrincipal user)
        {
            try
            {
        
                var product = await _repo.GetByIdAsync(dto.ProductId);

                if (product == null)
                {
                    return "產品不存在";
                }

                if (dto.Price < 0)
                {
                    return "價格不可小於0";

                }


                product.ProductName = dto.ProductName;
                product.Price = dto.Price;
                product.UpdatedDate = DateTime.Now;
                product.UpdatedBy = user.FindFirst(ClaimTypes.Name)?.Value;

                await _repo.UpdateAsync(product);
                _logger.LogInformation("產品更改成功");
                return "更改成功";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "產品更改失敗");
                throw;
            }

        }

       
        public async Task<string> DeleteProductAsync(string productId)
        {
            try
            {
                var product = await _repo.GetByIdAsync(productId);
                if (product == null)
                {
                    return "產品不存在";
                }
       
           
                var used = await _repo.ExistsInOrderAsync(productId);
                if (used)
                {
                    return "此產品已被訂單使用，無法刪除";
                }
            
                await _repo.DeleteAsync(product);
                _logger.LogInformation("產品刪除成功");
                return "刪除成功";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "產品刪除失敗");
                throw;
            }
        }

        public async Task<IPagedList<ProductDto>> GetPagedAsync(string? keyword, int pageNumber, int pageSize)
        {
            try
            {

                var query = _repo.GetQuery(keyword);

                var pagedData = await query
                    .OrderBy(x => x.ProductId)
                    .ToPagedListAsync(pageNumber, pageSize);

                var dtoList = pagedData.Select(x => new ProductDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Price = x.Price
                }).ToList();

                _logger.LogInformation("產品查詢成功");

                return new StaticPagedList<ProductDto>(
                dtoList,
                pagedData.PageNumber,
                pagedData.PageSize,
                pagedData.TotalItemCount
                );
            }
            catch (Exception ex)
            {
                _logger.LogError("ex, 產品查詢失敗");
                throw;
            }
            
        }
    }
}
