using Microsoft.AspNetCore.Authorization;

public class AuthorizationMiddleware
{
  private readonly RequestDelegate _next;

  public AuthorizationMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    var endpoint = context.GetEndpoint();
    var hasAuthorizeAttribute = endpoint?.Metadata.GetMetadata<IAuthorizeData>() != null;

    if (hasAuthorizeAttribute)
    {
      // Only enforce authentication if [Authorize] attribute is present
      if (!context.User.Identity?.IsAuthenticated ?? false)
      {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        await context.Response.WriteAsync("Unauthorized access (authentication required)");
        return;
      }
    }

    // Optional: Check roles/claims here
    if (!context.User.IsInRole("Admin"))
    {
    }

    await _next(context); // Call next middleware
  }
}
