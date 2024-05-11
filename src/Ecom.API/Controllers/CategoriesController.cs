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
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork unitOf;
        private readonly IMapper mapper;

        public CategoriesController(IUnitOfWork unitOf, IMapper mapper)
        {
            this.unitOf = unitOf;
            this.mapper = mapper;
        }

        [HttpGet("get-all-categories")]
        public async Task<ActionResult> GetAll()
        {
            var allCategories = await unitOf.CategoryRepository.GetAllAsync();
            
            if(allCategories is not null)
            {
                var res = mapper.Map<List<ListingCategoryDto>>(allCategories);
                return Ok(res);
            }
            return BadRequest("Not Found");
        }


        [HttpGet("get-categories-by-id/{id}")]
        public async Task<ActionResult> GetCategory(int id)
        {
            var category = await unitOf.CategoryRepository.GetByIdAsync(id);
            if(category is null)
            {
                return BadRequest($"Not Found This Id [{id}]");
            }
            var res = mapper.Map<ListingCategoryDto>(category);
            return Ok(res);
        }


        [HttpPost("add-new-category")]
        public async Task<ActionResult> Post(CategoryDto categoryDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Category category = mapper.Map<Category>(categoryDto);
                    await unitOf.CategoryRepository.AddAsync(category);
                    return Ok(categoryDto);
                }
                return BadRequest();
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("update-exiting-category-by-id/{id}")]
        public async Task<IActionResult> Put(int id, UpdateCategoryDto categoryDto)
        {
            try
            {
                if(id == categoryDto.Id)
                {
                    if (ModelState.IsValid)
                    {
                        var category = await unitOf.CategoryRepository.GetByIdAsync(categoryDto.Id);
                        if (category is not null)
                        {
                            mapper.Map(categoryDto, category);
                            await unitOf.CategoryRepository.UpdateAsync(categoryDto.Id, category);
                            return Ok(categoryDto);
                        }
                    }
                }
                return BadRequest($"Category Not Found, Id {id} Incorrect");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("delete-category-by-id/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var category = await unitOf.CategoryRepository.GetByIdAsync(id);
                if (category is not null)
                {
                    await unitOf.CategoryRepository.DeleteAsync(id);
                    return Ok($"This Category [{category.Name}] is Deleted Successfully");
                }
                return BadRequest("Not Found");
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
