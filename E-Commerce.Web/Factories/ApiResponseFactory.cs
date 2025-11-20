using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Web.Factories
{
    public static class ApiResponseFactory
    {
        public static IActionResult GenerateApiValidationErrorResponse(ActionContext actionContext)
        {
            var errors = actionContext.ModelState.Where(x => x.Value.Errors.Count > 0)
                                                  .ToDictionary(
                                                      kvp => kvp.Key,
                                                      kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                                                  );

            var Problem = new ProblemDetails
            {
                Title = "Validation Errors",
                Status = StatusCodes.Status400BadRequest,
                Detail = "One or more validation errors occurred.",
                Extensions = 
                {
                    {"errors",errors }
                }
            };

            return new BadRequestObjectResult(Problem);
        }
    }
}
