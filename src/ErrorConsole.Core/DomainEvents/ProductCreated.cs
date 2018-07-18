using System;

namespace ErrorConsole.Core.DomainEvents
{
    public class ProductCreated: DomainEvent
    {
        public ProductCreated(Guid productId, Guid companyId, string imageUrl, float price, string name, string description)
        {
            ProductId = productId;
            CompanyId = companyId;
            ImageUrl = imageUrl;
            Price = price;
            Name = name;
            Description = description;
        }

        public Guid ProductId { get; set; }
        public Guid CompanyId { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
