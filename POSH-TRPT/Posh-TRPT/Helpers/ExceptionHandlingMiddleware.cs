using Posh_TRPT_Models.DTO.API;
using Posh_TRPT_Utility.Resources;
using System.Net;
using System.Text.Json;

namespace Posh_TRPT.Helpers
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                if (httpContext.Session != null && !httpContext.Session.IsAvailable)
                {
                    httpContext.Response.Redirect("/Account/SignIn"); 
                    return;
                }

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = Configuration.ContentType;
            var response = context.Response;

            var errorResponse = new APIResponse<string>()
            {
                Success = false,
            };
            switch (exception)
            {
                case ApplicationException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message;
                    errorResponse.Status = HttpStatusCode.BadRequest;
                    errorResponse.Error = new CustomException(ex.Message, ex.InnerException);
                    break;
                case KeyNotFoundException ex:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponse.Message = ex.Message;
                    errorResponse.Status = HttpStatusCode.NotFound;
                    errorResponse.Error = new CustomException(ex.Message, ex.InnerException);
                    break;
                case Stripe.StripeException ex:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorResponse.Message = ex.Message+" "+GlobalResourceFile.OperationCancelled;
                    errorResponse.Status = HttpStatusCode.BadRequest;
                    errorResponse.Error = new CustomException(ex.Message, ex.InnerException);
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = Configuration.InternalServerError;
                    errorResponse.Status = HttpStatusCode.InternalServerError;
                    errorResponse.Error = new CustomException(exception.Message, exception.InnerException);
                    break;
            }
            _logger.LogError(exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
