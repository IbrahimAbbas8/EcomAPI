using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly EcomDbContext context;
        private readonly IFileProvider fileProvider;
        private readonly IMapper mapper;

        public ProductRepository(EcomDbContext context, IFileProvider fileProvider, IMapper mapper) : base(context)
        {
            this.context = context;
            this.fileProvider = fileProvider;
            this.mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateProductDto dto)
        {
            if (dto.Image is not null)
            {
                var root = "/images/Products/";
                var ProductName = $"{Guid.NewGuid()}" + dto.Image.FileName;
                if (!Directory.Exists("wwwroot" + root))
                {
                    Directory.CreateDirectory("wwwroot" + root);
                }
                var src = root + ProductName;
                var picInfo = fileProvider.GetFileInfo(src);
                var rootPath = picInfo.PhysicalPath;
                await Console.Out.WriteLineAsync(rootPath);
                using (var fileStream = new FileStream(rootPath, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(fileStream);
                }

                var res = mapper.Map<Product>(dto);
                res.ProductPicture = src;
                await context.Products.AddAsync(res);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<bool> UpdateAsync(int id, UpdateProductDto dto)
        {
            var currentProduct = await context.Products.FindAsync(id);
            if (currentProduct is not null)
            {
                var src = "";
                if (dto.Image is not null)
                {
                    var root = "/images/Products/";
                    var ProductName = $"{Guid.NewGuid()}" + dto.Image.FileName;
                    if (!Directory.Exists("wwwroot" + root))
                    {
                        Directory.CreateDirectory("wwwroot" + root);
                    }
                    src = root + ProductName;
                    var picInfo = fileProvider.GetFileInfo(src);
                    var rootPath = picInfo.PhysicalPath;
                    await Console.Out.WriteLineAsync(rootPath);
                    using (var fileStream = new FileStream(rootPath, FileMode.Create))
                    {
                        await dto.Image.CopyToAsync(fileStream);
                    }
                }

                // Remove Old ProductPicture
                if (!string.IsNullOrEmpty(currentProduct.ProductPicture))
                {
                    var picInfo = fileProvider.GetFileInfo(currentProduct.ProductPicture);
                    var rootPath = picInfo.PhysicalPath;
                    System.IO.File.Delete(rootPath);
                }

                // Update Product
                mapper.Map(dto, currentProduct);
                currentProduct.ProductPicture = src;
                context.Update(currentProduct);
                await context.SaveChangesAsync();

                return true;
            }
            return false;

        }


        public async Task<bool> DeleteAsyncWithImage(int id)
        {
            var res = await context.Products.FindAsync(id);

            if(res is not null)
            {
                if (!string.IsNullOrEmpty(res.ProductPicture))
                {
                    var picInfo = fileProvider.GetFileInfo(res.ProductPicture);
                    var rootPath = picInfo.PhysicalPath;
                    System.IO.File.Delete(rootPath);
                }

                context.Products.Remove(res);
                context.SaveChanges();
                return true;
            }
            return false;

        }
    }
}
