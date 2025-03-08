using AutoMapper;
using Ecom.API.Errors;
using Ecom.API.Extensions;
using Ecom.Core.Dtos;
using Ecom.Core.Entities;
using Ecom.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Security.Claims;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ITokenServices tokenServices;
        private readonly IMapper mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenServices tokenServices, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.tokenServices = tokenServices;
            this.mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if(ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    return Unauthorized("not authorize");
                }

                var res = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
                if (res is null || res.Succeeded == false)
                {
                    return Unauthorized("not authorize");
                }

                return Ok(new UserDto
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = tokenServices.CreateToken(user),
                });
                
            }
            return BadRequest();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!CheckEmailExist(dto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[]
                    {
                        "This Email is Already Token"
                    }
                });
            }
            var user = new AppUser
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                UserName = dto.Email,
            };
            var res = await userManager.CreateAsync(user, dto.Password);
            if (res.Succeeded is false)
            {
                BadRequest();
            }
            return Ok(new UserDto
            {
                DisplayName = dto.DisplayName,
                Email = dto.Email,
                Token = tokenServices.CreateToken(user),
            });
        }

        [Authorize]
        [HttpGet("hi")]
        public async Task<string> hi()
        {
            return "hi";
        }

        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            /*var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var user = await userManager.FindByEmailAsync(email);*/
            var user = await userManager.FindEmailByClaimPrincipal(HttpContext.User);
            return Ok(new UserDto
            {
                DisplayName = user?.DisplayName,
                Email = user?.Email,
                Token = tokenServices.CreateToken(user)
            });
        }

        //[Authorize]
        [HttpGet("check-email-exist")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
        {
            return Ok(await userManager.FindByEmailAsync(email) != null);
        }

        [Authorize]
        [HttpGet("get-user-address")]
        public async Task<IActionResult> GetUserAddress()
        {
            /*var email = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var user = await userManager.Users.Include(x => x.Address).SingleOrDefaultAsync(x => x.Email == email);*/
            var user = await userManager.FindUserByClaimPrincipalWithAdderss(HttpContext.User);
            var address = mapper.Map<AdderssDto>(user?.Address);
            return Ok(address);
        }

        [Authorize]
        [HttpPut("update-user-address")]
        public async Task<IActionResult> UpdateUserProfile(AdderssDto dto)
        {
            var user = await userManager.FindUserByClaimPrincipalWithAdderss(HttpContext.User);
            user.Address = mapper.Map<Address>(dto);
            var res = await userManager.UpdateAsync(user);
            if (res.Succeeded) return Ok(mapper.Map<AdderssDto>(user.Address));
            return BadRequest(res.Errors);
        }
    }
}
