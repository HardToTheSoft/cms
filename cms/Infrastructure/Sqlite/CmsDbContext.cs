using Microsoft.EntityFrameworkCore;

using Cms.Infrastructure.Entities;


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
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Entity>(entity =>
    {
      entity.ToTable("Entities");

      entity.HasKey(e => e.Id);

      entity.Property(e => e.CreatedAt)
        .HasDefaultValueSql(TIMESTAMP_DEFAULT_VALUE)
        .HasConversion(new DateTimeOffsetToLongValueConverter(true))
        .IsRequired(true);

      entity.Property(e => e.PayloadJson)
        .HasConversion(new JsonDocumentToJsonStringValueConverter(false))
        .IsRequired(false);

      entity.Property(e => e.UpdatedAt)
        .HasDefaultValueSql(TIMESTAMP_DEFAULT_VALUE)
        .HasConversion(new DateTimeOffsetToLongValueConverter(true))
        .IsRequired(true);
    });
  }
  #endregion
}