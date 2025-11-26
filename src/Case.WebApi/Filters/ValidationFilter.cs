using Case.WebApi.Middlewares;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using System.Text.Json;

namespace Case.WebApi.Filters
{
    public class ValidationFilter : IActionFilter
    {
        #region OnActionExecuted

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // OnActionExecuted
        }

        #endregion OnActionExecuted

        #region OnActionExecuting

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ActionArguments.Values.Count == 0)
                return;

            var argument = context.ActionArguments.Values.First();

            var generic = typeof(IValidator<>);
            var genericOfType = generic.MakeGenericType(argument.GetType());

            var validator = (IValidator)context.HttpContext.RequestServices.GetService(genericOfType);

            if (validator == null)
                return;

            var arg = new ValidationContext<object>(argument);
            var validationResult = validator.Validate(arg);

            if (validationResult.IsValid)
                return;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var errors = validationResult.Errors.Select(x => new ErroViewModel(x.ErrorCode, x.ErrorMessage));

            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Result = new JsonResult(errors, options);
        }
    }

    #endregion OnActionExecuting
}