using Ecom.API.Errors;
using Ecom.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BugController : ControllerBase
    {
        private readonly EcomDbContext context;

        public BugController(EcomDbContext context)
        {
            this.context = context;
        }

        [HttpGet("not-found")]
        public ActionResult GetNotFound()
        {
            var product = context.Products.Find(50);
            if (product == null)
            {
                return NotFound(new BaseCommonResponse(404));
            }
            return Ok(product);
        }

        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            var product = context.Products.Find(50);
            product.Name = "";
            return Ok();
        }

        [HttpGet("bad-request/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }
        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new BaseCommonResponse(400));
        }
    }
}
