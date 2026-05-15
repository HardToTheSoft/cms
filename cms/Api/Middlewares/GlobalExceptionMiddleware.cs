using System.Text.Json;

using Microsoft.AspNetCore.Mvc;


public class GlobalExceptionMiddleware
{
  #region Members
  private readonly RequestDelegate _next;

  private readonly ILogger<GlobalExceptionMiddleware> _logger;

  private readonly IWebHostEnvironment _env;

  #endregion


  #region Constructor
  public GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger,
    IWebHostEnvironment env)
  {
    _next = next;
    _logger = logger;
    _env = env;
  }
  #endregion


  #region Public methods
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Unhandled exception occurred");

      await HandleExceptionAsync(context, ex);
    }
  }
  #endregion


  #region Private methods
  private async Task HandleExceptionAsync(HttpContext context, Exception ex)
  {
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = StatusCodes.Status500InternalServerError;

    var problemDetails = new ProblemDetails
    {
      Status = context.Response.StatusCode,
      Title = "Internal Server Error",
      Detail = _env.IsDevelopment() ? ex.Message : "An unexpected error occurred.",
      Type = "https://httpstatuses.com/500"
    };

    await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
  }
  #endregion
}