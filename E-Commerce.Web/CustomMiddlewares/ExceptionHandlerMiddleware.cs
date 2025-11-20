using E_Commerce.Services.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.CustomMiddlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate Next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = Next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext); // Call the next middleware in the pipeline
                await HandleNotFoundEndPointAsync(httpContext);
            }
            catch (Exception ex)
            {
                // Logging The Exception 
                _logger.LogError($"Something went wrong: {ex}");
                // default Place to logger is Console but you can change it to File, DB, etc. 

                // Return a Custom Error Response

                var Problem = new ProblemDetails() // Request Body
                {
                    Title = "An unexpected error occurred!",
                    Detail = ex.Message,
                    Instance = httpContext.Request.Path,
                    Status = ex switch
                    { 
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError

                    },
                };
                httpContext.Response.StatusCode = Problem.Status.Value;
               await httpContext.Response.WriteAsJsonAsync(Problem);  // Serializing the ProblemDetails object to JSON and writing it to the response body
            }

        
        }

        private static async Task HandleNotFoundEndPointAsync(HttpContext httpContext)
        {
            if (httpContext.Response.StatusCode == StatusCodes.Status404NotFound)
            {
                var Problem = new ProblemDetails()
                {
                    Title = "The resource you are looking for was not found.",
                    Status = StatusCodes.Status404NotFound,
                    Detail = $"EndPoint {httpContext.Request.Path} Not Found",
                    Instance = httpContext.Request.Path
                };
                await httpContext.Response.WriteAsJsonAsync(Problem); // Serializing the ProblemDetails object to JSON and writing it to the response body
            }
        }
    }
}
