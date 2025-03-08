using AutoMapper;
using Ecom.API.Errors;
using Ecom.Core.Dtos;
using Ecom.Core.Entities.Orders;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork uow;
        private readonly IOrderServices orderServices;
        private readonly IMapper mapper;

        public OrdersController(IUnitOfWork uow, IOrderServices orderServices, IMapper mapper)
        {
            this.uow = uow;
            this.orderServices = orderServices;
            this.mapper = mapper;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var address = mapper.Map<AdderssDto, ShipAddress>(orderDto.ShipToAddress);
            var order = await orderServices.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);
            if (order == null) return BadRequest(new BaseCommonResponse(400, "Error While Creating Order"));
            return Ok(order);
        }
    }
}
