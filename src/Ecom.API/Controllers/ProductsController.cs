﻿using AutoMapper;
using Ecom.API.Errors;
using Ecom.API.Helper;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork unitOf;
        private readonly IMapper mapper;

        public ProductsController(IUnitOfWork unitOf, IMapper mapper)
        {
            this.unitOf = unitOf;
            this.mapper = mapper;
        }

        [HttpGet("get-all-product")]
        public async Task<ActionResult> Get([FromQuery]ProductParams Params)
        {
            var products = await unitOf.ProductRepository.GetAllAsync(Params);
            if(products is not null)
            {
               // var res = mapper.Map<List<ProductDto>>(products);
                return Ok(new Pagination<ProductDto>(Params.PageNumber ,Params.PageSize, products.TotalItems, products.ProductDtos));
            }
            return BadRequest();
        }

        [HttpGet("get-product-by-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseCommonResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> GetById(int id)
        {
            var product = await unitOf.ProductRepository.GetByIdAsync(id, p => p.Category);
            if(product is not null)
            {
                var res = mapper.Map<ProductDto>(product);
                return Ok(res);
            }
            return NotFound(new BaseCommonResponse(404));
        }

        [HttpPost("add-new-product")]
        public async Task<ActionResult> Post(CreateProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await unitOf.ProductRepository.AddAsync(productDto);
                    return res ? Ok(productDto) : BadRequest();
                }
                return BadRequest(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }


        [HttpPut("update-exiting-product-by-id/{id}")]
        public async Task<ActionResult> Put(int id, UpdateProductDto productDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await unitOf.ProductRepository.UpdateAsync(id, productDto);
                    return res ? Ok(productDto) : BadRequest(res);
                }
                return BadRequest(productDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete-product-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var res = await unitOf.ProductRepository.DeleteAsyncWithImage(id);
                    return res ? Ok(res) : BadRequest(res);
                }
                return NotFound($"Not Found This Id [{id}]");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
