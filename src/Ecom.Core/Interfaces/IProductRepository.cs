using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Sharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Interfaces
{
    public interface IProductRepository: IGenericRepository<Product>
    {
        Task<ReturnProductDto> GetAllAsync(ProductParams Params);
        Task<bool> AddAsync(CreateProductDto productDto);
        Task<bool> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsyncWithImage(int id);
    }
}
