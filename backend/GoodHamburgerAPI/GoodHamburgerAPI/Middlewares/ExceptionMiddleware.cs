using System.Net;
using System.Text.Json;
using GoodHamburger.Domain.Exceptions;

namespace GoodHamburgerAPI.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (OrderNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                await WriteResponseAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (DuplicateItemTypeException ex)
            {
                _logger.LogWarning(ex.Message);
                await WriteResponseAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex.Message);
                await WriteResponseAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado.");
                await WriteResponseAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro interno. Tente novamente mais tarde.");
            }
        }

        private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var response = new { error = message };
            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}
