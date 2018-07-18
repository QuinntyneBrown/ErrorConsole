using ErrorConsole.Core.Models;
using System;

namespace ErrorConsole.API.Features.Products
{
    public class ProductDto
    {        
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public string ImageUrl { get; set; }
        public Guid CompanyId { get; set; }
        public static ProductDto FromProduct(Product product)
        {
            var model = new ProductDto();
            model.ProductId = product.ProductId;
            model.CompanyId = product.CompanyId;
            model.Name = product.Name;
            model.Description = product.Description;
            model.Price = product.Price;
            model.ImageUrl = product.ImageUrl;
            return model;
        }
    }
}
