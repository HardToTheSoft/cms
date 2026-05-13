using Microsoft.OpenApi;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;

using Cms.Infrastructure;
using Cms.Infrastructure.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

builder.Services.AddControllers();

builder.Services.AddAuthentication(BasicAuthenticationHandler.SchemeName)
  .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationHandler.SchemeName, null);

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
  var openApiSecurityScheme = new OpenApiSecurityScheme
  {
    Type = SecuritySchemeType.Http,
    Scheme = BasicAuthenticationHandler.SchemeName,
    In = ParameterLocation.Header,
    Name = HeaderNames.Authorization,
    Description = "Basic Authentication"
  };

  options.AddSecurityDefinition(BasicAuthenticationHandler.SchemeName, openApiSecurityScheme);

  options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
  {
    [new OpenApiSecuritySchemeReference(BasicAuthenticationHandler.SchemeName, document)] = []
  });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();