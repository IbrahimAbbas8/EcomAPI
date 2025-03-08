using AutoMapper;
using Ecom.Core.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.Extensions.FileProviders;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EcomDbContext context;
        private readonly IFileProvider fileProvider;
        private readonly IMapper mapper;

        public ICategoryRepository CategoryRepository { get; }

        public IProductRepository ProductRepository { get; }

        public IBasketRepository BasketRepository {  get; }

        public UnitOfWork(EcomDbContext context, IFileProvider fileProvider, IMapper mapper, IConnectionMultiplexer redis)
        {
            this.context = context;
            this.fileProvider = fileProvider;
            this.mapper = mapper;
            CategoryRepository = new CategoryRepository(context);
            ProductRepository = new ProductRepository(context, this.fileProvider, this.mapper);
            BasketRepository = new BasketRepository(redis, mapper);
        }

        
    }
}
