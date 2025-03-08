using AutoMapper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IUnitOfWork unitOf;
        private readonly HttpClient http;
        private readonly IMapper mapper;

        public BasketController(IUnitOfWork unitOf, HttpClient http, IMapper mapper)
        {
            this.unitOf = unitOf;
            this.http = http;
            this.mapper = mapper;
        }

        [HttpGet("get-basket-item/{id}")]
        public async Task<IActionResult> GetBasketById(string id)
        {
            var basket = await unitOf.BasketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost("update-basket")]
        public async Task<IActionResult> UpdateBasket(CustomerBasketDto customerBasket)
        {
            var res = mapper.Map<CustomerBasket>(customerBasket);
            var basket = await unitOf.BasketRepository.UpdateBasketAsync(res);
            return Ok(basket);
        }

        [HttpDelete("delete-basket-item/{id}")]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            var basket = await unitOf.BasketRepository.DeleteBasketAsync(id);
            return Ok(basket);
        }

        /*[HttpGet]
        public async Task<IActionResult> ddd()
        {
            var x = await http.GetAsync("https://cdn.jsdelivr.net/gh/fawazahmed0/quran-api@1/editions/ara-quranindopak/10/10.json");
            return Ok(await x.Content.ReadAsStringAsync());
        }*/
    }
}
