using TaskManagerAPI.Exceptions;

namespace TaskManagerAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next) 
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            { 
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (UnauthorizedException ex)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsJsonAsync(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { message = "An unexpected error occurred" });
            }
            

        }

    }
}
