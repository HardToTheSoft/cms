using Microsoft.OpenApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

using Cms.Infrastructure;
using Cms.Infrastructure.Services;


var webApplicationBuilder = WebApplication.CreateBuilder(args);

webApplicationBuilder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

webApplicationBuilder.Services.AddControllers(options =>
  {
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
  });

webApplicationBuilder.Services.AddAuthentication(BasicAuthenticationHandler.SCHEME_NAME)
  .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationHandler.SCHEME_NAME, null);

webApplicationBuilder.Services.AddAuthorization();

webApplicationBuilder.Services.AddEndpointsApiExplorer();

webApplicationBuilder.Services.AddSwaggerGen(options =>
{
  var openApiSecurityScheme = new OpenApiSecurityScheme
  {
    Type = SecuritySchemeType.Http,
    Scheme = BasicAuthenticationHandler.SCHEME_NAME,
    In = ParameterLocation.Header,
    Name = HeaderNames.Authorization,
    Description = "Basic Authentication"
  };

  options.AddSecurityDefinition(BasicAuthenticationHandler.SCHEME_NAME, openApiSecurityScheme);

  options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
  {
    [new OpenApiSecuritySchemeReference(BasicAuthenticationHandler.SCHEME_NAME, document)] = []
  });
});

var webApplication = webApplicationBuilder.Build();

webApplication.UseMiddleware<GlobalExceptionMiddleware>();

if (webApplication.Environment.IsDevelopment())
{
  webApplication.UseSwagger();
  webApplication.UseSwaggerUI();
}

webApplication.UseHttpsRedirection();
webApplication.UseRouting();
webApplication.UseAuthentication();
webApplication.UseAuthorization();
webApplication.MapControllers();

webApplication.Run();