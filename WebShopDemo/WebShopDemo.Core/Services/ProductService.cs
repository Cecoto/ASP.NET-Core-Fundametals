﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Core.Data.Common;
using WebShopDemo.Core.Data.Models;
using WebShopDemo.Core.Models;

namespace WebShopDemo.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration config;
        private readonly IRepository repo;
        /// <summary>
        /// IoC
        /// </summary>
        /// <param name="_config">Application config</param>
        public ProductService(
            IConfiguration _config,
            IRepository _repo)
        {
            config = _config;
            repo = _repo;
        }

        /// <summary>
        /// Add new product
        /// </summary>
        /// <param name="productDto">Product model</param>
        public async Task Add(ProductDto productDto)
        {
            var product = new Product()
            {
                Name = productDto.Name,
                Price = productDto.Price,
                Quantity = productDto.Quantity
            };

            await repo.AddAsync(product);
            await repo.SaveChangesAsync();

        }

        /// <summary>
        /// Buying product of the list of products
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Buy(Guid id)
        {
            var product = await repo.GetByIdAsync<Product>(id);

            if (product!=null)
            {
                product.IsActive = false;

                await repo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// deleting product from the product list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(Guid id)
        {
            var product = await repo.All<Product>()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product!=null)
            {
                product.IsActive = false;

                await repo.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>List of products</returns>
        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            return await repo.AllReadonly<Product>()
                .Where(p => p.IsActive)
                .Select(p => new ProductDto()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity
                })
                .ToListAsync();
        }

    }
}
