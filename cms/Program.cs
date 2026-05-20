using System.Reflection;
using Microsoft.OpenApi;
using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

using Cms.Api.Middlewares;
using Cms.Api.Authentication;

using Cms.Application.Services;
using Cms.Application.Extensions;
using Cms.Application.Interfaces;

using Cms.Domain;

using Cms.Infrastructure.Services;
using Cms.Infrastructure.Persistence;
using Cms.Infrastructure.EventHandler;
using Cms.Infrastructure.Persistence.Services;
using Cms.Infrastructure.Persistence.Repositories;


var webApplicationBuilder = WebApplication.CreateBuilder(args);

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

var connectionString = webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection");

var sqliteConnectionStringBuilder = new SqliteConnectionStringBuilder(connectionString);

var dbBaseDirectory = Path.GetDirectoryName(sqliteConnectionStringBuilder.DataSource);

if (!string.IsNullOrEmpty(dbBaseDirectory) && !Directory.Exists(dbBaseDirectory))
  Directory.CreateDirectory(dbBaseDirectory);

webApplicationBuilder.Services.AddDbContext<SqliteDbContext>(options => options.UseSqlite(connectionString));

webApplicationBuilder.Services.AddHostedService<EventDispatcherBackgroundService>();

webApplicationBuilder.Services.AddHttpContextAccessor();

webApplicationBuilder.Services.AddSingleton<IEventQueueService, EventQueueService>();

webApplicationBuilder.Services.AddEventHandlers(Assembly.GetExecutingAssembly());

webApplicationBuilder.Services.AddScoped<IUserContext, UserContextService>();
webApplicationBuilder.Services.AddScoped<IEventDispatcherService, EventDispatcherService>();
webApplicationBuilder.Services.AddScoped<IEntityService, EntityService>();
webApplicationBuilder.Services.AddScoped<IEntityRepository<Entity>, EntityRepositoryService>();
webApplicationBuilder.Services.AddScoped(typeof(IRepository<>), typeof(RepositoryService<>));

webApplicationBuilder.Services.AddTransient<ISearchRepositoryService<EntityRepository>, SearchRepositoryService>();

var webApplication = webApplicationBuilder.Build();

webApplication.UseMiddleware<GlobalExceptionMiddleware>();

if (webApplication.Environment.IsDevelopment())
{
  webApplication.UseSwagger();
  webApplication.UseSwaggerUI();

  //webApplication.UseMiddleware<EnableRequestBodyBufferingMiddleware>();

  // using var serviceScope = webApplication.Services.CreateScope();

  // var dbContext = serviceScope.ServiceProvider.GetRequiredService<CmsDbContext>();

  // dbContext.Database.Migrate();
}

webApplication.UseHttpsRedirection();
webApplication.UseRouting();
webApplication.UseAuthentication();
webApplication.UseAuthorization();
webApplication.MapControllers();

webApplication.Run();