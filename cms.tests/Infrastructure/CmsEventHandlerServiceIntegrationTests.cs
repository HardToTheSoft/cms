using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using Cms.Infrastructure.Persistence;


namespace Cms.Tests.Infrastructure;


public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
  #region Members
  private SqliteConnection _sqliteConnection;
  #endregion


  #region Private methods
  protected override void ConfigureWebHost(Microsoft.AspNetCore.Hosting.IWebHostBuilder builder)
  {
    builder.ConfigureServices(services =>
    {
      var serviceDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<SqliteDbContext>));

      if (serviceDescriptor is not null)
        services.Remove(serviceDescriptor);

      _sqliteConnection = new SqliteConnection("DataSource=:memory:");

      _sqliteConnection.Open();

      services.AddDbContext<SqliteDbContext>(options =>
        {
          options.UseSqlite(_sqliteConnection);
        });

      var serviceProvider = services.BuildServiceProvider();

      using var serviceScope = serviceProvider.CreateScope();

      var dbContext = serviceScope.ServiceProvider.GetRequiredService<SqliteDbContext>();

      dbContext.Database.Migrate();
    });
  }


  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);

    if (disposing)
    {
      _sqliteConnection?.Close();
      _sqliteConnection?.Dispose();
    }
  }
  #endregion
}