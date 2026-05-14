public sealed class EnableRequestBodyBufferingMiddleware
{
  #region Members
  private readonly RequestDelegate _next;
  #endregion


  #region Constructor
  public EnableRequestBodyBufferingMiddleware(RequestDelegate next)
  {
    _next = next;
  }
  #endregion


  #region Public methods
  public async Task InvokeAsync(HttpContext context)
  {
    context.Request.EnableBuffering();
    
    await _next(context);
  }
  #endregion
}