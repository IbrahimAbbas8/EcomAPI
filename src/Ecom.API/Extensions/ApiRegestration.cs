using Ecom.API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Extensions
{
    public static class ApiRegestration
    {
        public static IServiceCollection AddApiRegestration(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(opt =>
              {
                  opt.InvalidModelStateResponseFactory = context =>
                  {
                      var errorResponse = new ApiValidationErrorResponse
                      {
                          Errors = context.ModelState
                                      .Where(x => x.Value.Errors.Count() > 0)
                                      .SelectMany(x => x.Value.Errors)
                                      .Select(x => x.ErrorMessage).ToArray()
                      };
                      return new BadRequestObjectResult(errorResponse);
                  };
              });
            return services;
        }
    }
}
