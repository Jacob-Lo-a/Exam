using Exam.Core.DTOs;
using Exam.Core.interfaces;
using Exam.Core.Models;
using System.Security.Claims;
using X.PagedList;
namespace Exam.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<string> CreateProductAsync(
            CreateProductDto dto,
            ClaimsPrincipal user)
        {
           
            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            

            
            if (dto.Price < 0)
                return "價格不可小於0";

           
            if (await _repo.ExistsAsync(dto.ProductName))
                return "產品名稱已存在";

           
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

            return "建立成功";
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
          
            var role = user.FindFirst(ClaimTypes.Role)?.Value;
            

            
            var product = await _repo.GetByIdAsync(dto.ProductId);
            if (product == null)
                return "產品不存在";

            
            if (dto.Price < 0)
                return "價格不可小於0";

            
            product.ProductName = dto.ProductName;
            product.Price = dto.Price;
            product.UpdatedDate = DateTime.Now;
            product.UpdatedBy = user.FindFirst(ClaimTypes.Name)?.Value;

            await _repo.UpdateAsync(product);

            return "更新成功";
        }

       
        public async Task<string> DeleteProductAsync(string productId)
        {
                                   
            var product = await _repo.GetByIdAsync(productId);
            if (product == null)
                return "產品不存在";

           
            var used = await _repo.ExistsInOrderAsync(productId);
            if (used)
                return "此產品已被訂單使用，無法刪除";

           
            await _repo.DeleteAsync(product);

            return "刪除成功";
        }

        public async Task<IPagedList<ProductDto>> GetPagedAsync(string? keyword, int pageNumber, int pageSize)
        {
            var data = await _repo.GetPagedAsync(keyword, pageNumber, pageSize);

            var dtoList = data.Select(x => new ProductDto
            {
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Price = x.Price
            }).ToList();

            return new StaticPagedList<ProductDto>(
                dtoList,
                data.PageNumber,
                data.PageSize,
                data.TotalItemCount
            );
        }
    }
}
