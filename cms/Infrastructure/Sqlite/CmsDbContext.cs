using Microsoft.EntityFrameworkCore;

using Cms.Data;
using System.Text.Json;


namespace Cms.Infrastructure.Sqlite;


public sealed class CmsDbContext : DbContext
{
  #region Members
  private const string TIMESTAMP_DEFAULT_VALUE = "(strftime('%s','now') * 1000)";
  #endregion


  #region Constructor
  public CmsDbContext(DbContextOptions<CmsDbContext> options)
    : base(options) { }
  #endregion


  #region Properties
  public DbSet<Entity> Entities { get; set; }
  #endregion


  #region Private methods
  protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
  {
    configurationBuilder.Properties<DateTimeOffset>()
      .HaveConversion<DateTimeOffsetToLongValueConverter>();

    configurationBuilder.Properties<JsonElement>()
      .HaveConversion<JObjectToJsonStringValueConverter>();
  }


  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Entity>(entity =>
    {
      entity.ToTable("Entities");

      entity.HasKey(e => e.Id);

      entity.Property(e => e.CreatedAt)
        .HasDefaultValueSql(TIMESTAMP_DEFAULT_VALUE);

      entity.Property(e => e.UpdatedAt)
        .HasDefaultValueSql(TIMESTAMP_DEFAULT_VALUE);
    });
  }
  #endregion
}