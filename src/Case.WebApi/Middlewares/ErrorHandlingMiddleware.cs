using Case.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Case.WebApi.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                await HandleDomainExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            const string errorMessage = "INTERNAL SERVER ERROR";
            const string message = "Oops, internal server error";

            Log.Error(exception, $"{message} | {exception.Message}");

            var erros = new List<ErroViewModel>
                {
                    new ErroViewModel(errorMessage, message)
                };

            return HandleResponseMessageAsync(context, erros, HttpStatusCode.InternalServerError);
        }

        private static Task HandleDomainExceptionAsync(HttpContext context, DomainException exception)
        {
            var erros = new List<ErroViewModel>()
                    {
                        new ErroViewModel(exception.Message,exception.ErrorCode)
                    };

            return HandleResponseMessageAsync(context, erros, HttpStatusCode.BadRequest);
        }

        private static Task HandleResponseMessageAsync(HttpContext context, IEnumerable<ErroViewModel> erros, HttpStatusCode httpStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var result = JsonSerializer.Serialize(erros, options);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;
            return context.Response.WriteAsync(result);
        }
    }
}