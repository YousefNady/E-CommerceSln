using E_Commerce.Services_Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace E_Commerce.Presentation.Attributes
{
    public class RedisCacheAttribute : ActionFilterAttribute
    {
        readonly int _DurationInMin;
        public RedisCacheAttribute(int DurationInMin = 5) // Default Cache Duration Is 5 Minutes & Also I give The Ability To Change It When I Use The Attribute
        {
            _DurationInMin = DurationInMin;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) // Before And After The Action Method Is Executed
        {
            // Get Cache Service From DI Container
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>(); // CLR Will Resolve The Service From The DI Container

            // Create Cache Key Based On Request Path And Query String
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            // Check If Cached Data Exists
            var cacheValue = await cacheService.GetAsync(cacheKey);
            // If Exists, Return Cached Data and Skip Executing Of EndPoint
            if (cacheValue is not null) // Cache Hit ||  Cached Data Exists Before
            {
                context.Result = new ContentResult()
                {
                    Content = cacheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };

            }
            // If Not Exists, Execute The EndPoint and Store The Result In Cache if 200 OK Response
           var ExecutedResult = await next.Invoke(); // Execute The EndPoint => it may be holds the address of the action method or the next filter in the pipeline
            if (ExecutedResult.Result is OkObjectResult result) // (is) to check the Type then cast it and stored in result
            {
              await cacheService.SetAsync(cacheKey, result.Value!, TimeSpan.FromMinutes(_DurationInMin));
            }
        }


        private string GenerateCacheKeyFromRequest(HttpRequest Request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append(Request.Path); // api/Products 
            foreach (var item in Request.Query.OrderBy(x => x.Key))
            {
                Key.Append($"|{item.Key}-{item.Value}"); // api/Products | brandId-2|typeId-1
            }
            return Key.ToString();
        }
    }
}
