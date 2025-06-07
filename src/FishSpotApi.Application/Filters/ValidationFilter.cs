using FishSpotApi.Domain.Http;
using FishSpotApi.Domain.Resources;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Localization;

namespace FishSpotApi.Application.Filters;

public class ValidationFilter(IStringLocalizerFactory factory) : Attribute, IActionFilter
{
    private readonly IStringLocalizer _localizer = factory.Create(typeof(FishSpotResource));

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
            Message = _localizer["unauthorized"],
            Error = errors
        };

        context.Result = new BadRequestObjectResult(http);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // OnActionExecuted method not used
    }

    private List<ErrorResponse> ErrorConverter(ModelStateDictionary model)
    {
        if (model.ErrorCount <= 0)
        {
            return [];
        }

        var errors = new List<ErrorResponse>();
        foreach (var (field, value) in model)
        {
            var error = value.Errors.FirstOrDefault();
            errors.Add(new ErrorResponse()
            {
                Field = field,
                Message = _localizer[error?.ErrorMessage ?? string.Empty],
            });
        }

        return errors;
    }
}