﻿using Ecom.API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    [Route("errors/{statusCode}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        [HttpGet()]
        public IActionResult Error(int statusCode)
        {
            return new ObjectResult(new BaseCommonResponse(statusCode));
        }
    }
}
