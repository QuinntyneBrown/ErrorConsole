﻿using ErrorConsole.Core.Common;
using ErrorConsole.Core.DomainEvents;
using MediatR;
using System;

namespace ErrorConsole.Core.Models
{
    public class Product: AggregateRoot
    {
        public Product()
        {

        }

        public Product(Guid productId,Guid companyId, string imageUrl, float price, string name, string description)
        {
            Apply(new ProductCreated(productId,companyId,imageUrl,price,name,description));
        }
        public Guid ProductId { get; set; }
        public Guid CompanyId { get; set; }
        public string ImageUrl { get; set; }
        public float Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public void Remove() {

        }
        public override void Apply(DomainEvent @event)
        {
            switch(@event)
            {
                case ProductCreated productCreated:
                    ProductId = productCreated.ProductId;
                    ImageUrl = productCreated.ImageUrl;
                    Price = productCreated.Price;
                    Name = productCreated.Name;
                    Description = productCreated.Description;
                    CompanyId = productCreated.CompanyId;
                    break;
            }

            RaiseDomainEvent(@event);
        }
    }
}
