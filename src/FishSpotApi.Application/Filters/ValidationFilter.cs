using FishSpotApi.Domain.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FishSpotApi.Application.Filters;

public class ValidationFilter : Attribute, IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid)
        {
            return;
        }

        var errors = ErrorConverter(context.ModelState);
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "Don't authorized",
            Error = errors
        };
        
        context.Result = new BadRequestObjectResult(http);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // OnActionExecuted method not used
    }
    
    private static List<ErrorResponse> ErrorConverter(ModelStateDictionary model) 
    {
        if (model.ErrorCount <= 0) 
        {
            return [];
        }

        var errors = new List<ErrorResponse>();
        foreach (var (field, value) in model) 
        {
            var message = string.Empty;
            foreach (var error in value.Errors) 
            {
                message = error.ErrorMessage;
            }

            errors.Add(new ErrorResponse() 
            {
                Field = field,
                Message = message 
            });
        }

        return errors;
    }
}