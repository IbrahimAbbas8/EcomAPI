using Ecom.Core.Entities.Orders;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class OrderServices : IOrderServices
    {
        private readonly IUnitOfWork uow;
        private readonly EcomDbContext context;

        public OrderServices(IUnitOfWork uow, EcomDbContext context)
        {
            this.uow = uow;
            this.context = context;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, ShipAddress shipAddress)
        {
            var basket = await uow.BasketRepository.GetBasketAsync(basketId);
            var items = new List<OrderItem>();
            Parallel.ForEach(basket.BasketItems,async item =>
            {
                var productItem = await uow.ProductRepository.GetByIdAsync(item.Id);
                var productItemOrder = new ProductItemOrder(productItem.Id, productItem.Name, productItem.ProductPicture);
                var OrderItem = new OrderItem(productItemOrder, item.Price, item.Quantity);
                lock (items)
                {
                    items.Add(OrderItem);
                }
            });
            var deliveryMethod = await context.DeliveryMethods.FirstOrDefaultAsync(x => x.Id == deliveryMethodId);
            var subTotal = items.Sum(x => x.Price * x.Quantity);

            var order = new Order(buyerEmail, shipAddress ,deliveryMethod, items ,subTotal);
            if (order is null) return null;
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
            await uow.BasketRepository.DeleteBasketAsync(basketId);
            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
