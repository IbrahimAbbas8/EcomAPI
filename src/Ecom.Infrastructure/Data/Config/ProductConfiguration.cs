using Ecom.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Name).HasMaxLength(30);
            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            // builder.HasOne(c => c.Category).WithMany(p => p.Products).HasForeignKey(c => c.CategoryId);

            builder.HasData(
                new Product { Id = 1, Name = "Product_1", Description = "p1", Price = 2000, CategoryId = 1, ProductPicture = "http://"},
                new Product { Id = 2, Name = "Product_2", Description = "p2", Price = 3000, CategoryId = 2, ProductPicture = "http://" },
                new Product { Id = 3, Name = "Product_3", Description = "p3", Price = 4000, CategoryId = 1, ProductPicture = "http://" }
                );
        }
    }
}
