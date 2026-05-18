using System.Text.Json;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Cms.Api.Models;
using Cms.Api.Services;
using Cms.Tests.Infrastructure;
using Cms.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Cms.Infrastructure.Sqlite;


namespace Cms.Tests;


[TestClass]
public class CmsEventHandlerServiceIntegrationTests
{
  #region Members
  private WebApplicationFactory<Program> _webApplicationFactory;

  private TestLogger<CmsEventHandlerService> _logger;

  private CmsEventHandlerService _cmsEventHandler;
  #endregion


  #region Properties
  public TestContext TestContext { get; set; }
  #endregion


  #region Public methods
  [TestInitialize]
  public void Setup()
  {
    //_webApplicationFactory = new WebApplicationFactory<Program>();
    _webApplicationFactory = new MyWebApplicationFactory();

    _logger = new TestLogger<CmsEventHandlerService>(TestContext);
  }


  [TestMethod]
  public async Task CmsEventHandler_HandleEvent_LogsAndReturnsSuccess()
  {
    using var scope = _webApplicationFactory.Services.CreateScope();

    _cmsEventHandler = new CmsEventHandlerService(
      _logger,
      scope.ServiceProvider.GetService<IEntityService>());

    await _cmsEventHandler.HandleEventAsync(new CmsEventModel
    {
      Topic = CmsEventHandlerService.PROCESS_EVENTS_TOPIC,
      Payload = new List<EventModel>
      {
        new()
        {
          Id = "X",
          Type = "publish",
          Payload = JsonDocument.Parse("{\"example\": \"value\" }"),
          Version = 2,
          Timestamp = DateTimeOffset.Parse("2024-01-01T00:00:00Z")
        },
        new()
        {
          Id = "Y",
          Type = "delete",
          Timestamp = DateTimeOffset.Parse("2024-01-01T00:00:00Z")
        },

        new()
        {
          Id = "Z",
          Type = "unPublish",
          Payload = JsonDocument.Parse("{\"example\": \"value\" }"),
          Version = 4,
          Timestamp = DateTimeOffset.Parse("2024-01-01T00:00:00Z")
        }
      }
    });

    bool success = _logger.Messages.TryGetValue("SUCCESS", out _);

    Assert.IsTrue(success);
  }
  #endregion
}

public class MyWebApplicationFactory : WebApplicationFactory<Program>
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      var descriptor = services.SingleOrDefault(
              d => d.ServiceType == typeof(DbContextOptions<CmsDbContext>));

      services.Remove(descriptor);

      services.AddDbContext<CmsDbContext>(options =>
          {
            options.UseSqlite("DataSource=:memory:");
          });

      var sp = services.BuildServiceProvider();

      using var serviceScope = sp.CreateScope();

      var dbContext = serviceScope.ServiceProvider.GetRequiredService<CmsDbContext>();

      dbContext.Database.Migrate();
    });
  }
}