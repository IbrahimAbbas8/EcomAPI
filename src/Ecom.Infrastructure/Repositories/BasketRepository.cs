using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;
        private readonly IMapper mapper;

        public BasketRepository(IConnectionMultiplexer redis, IMapper mapper)
        {
            this.database = redis.GetDatabase();
            this.mapper = mapper;
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            var check = await database.KeyExistsAsync(BasketId);
            if(check) return await database.KeyDeleteAsync(BasketId);
            return false;
        }

        public async Task<CustomerBasket> GetBasketAsync(string BasketId)
        {
            var data = await database.StringGetAsync(BasketId);
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(data);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket customerBasket)
        {
            var basket = await database.StringSetAsync(customerBasket.Id,
                JsonSerializer.Serialize(customerBasket),TimeSpan.FromDays(30)
                );
            if (!basket) return null;
            return await GetBasketAsync(customerBasket.Id);
        }
    }
}
