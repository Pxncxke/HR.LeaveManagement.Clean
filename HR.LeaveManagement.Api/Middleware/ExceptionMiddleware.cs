using HR.LeaveManagement.Api.Models;
using HR.LeaveManagement.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
//using SendGrid.Helpers.Errors.Model;
using System.Net;

namespace HR.LeaveManagement.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
           HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            dynamic problem;

            switch(ex)
            {
                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    problem = new CustomValidationProblemDetails
                    {
                        Title = badRequestException.Message,
                        Status = (int)statusCode,
                        Detail = badRequestException.InnerException?.Message,
                        Type = nameof(BadRequestException),
                        Errors = badRequestException.ValidationErrores
                    };
                    break;
                case NotFoundException NotFound:
                    statusCode = HttpStatusCode.NotFound;
                    problem = new CustomValidationProblemDetails
                    {
                        Title = NotFound.Message,
                        Status = (int)statusCode,
                        Detail = NotFound.InnerException?.Message,
                        Type = nameof(NotFoundException),
                    };
                    break;
                default:
                  
                    problem = new CustomValidationProblemDetails
                    {
                        Title = ex.Message,
                        Status = (int)statusCode,
                        Detail = ex.StackTrace,
                        Type = nameof(HttpStatusCode.InternalServerError),
                    };
                    break;
            }
            httpContext.Response.StatusCode = (int)statusCode;
            var result = JsonConvert.SerializeObject((object)problem);
            
            await httpContext.Response.WriteAsJsonAsync(result);
        }
    }
}
