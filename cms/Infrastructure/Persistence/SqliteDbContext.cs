using Microsoft.EntityFrameworkCore;

using Cms.Infrastructure.Persistence.Repositories;


namespace Cms.Infrastructure.Persistence;


public sealed class SqliteDbContext : DbContext
{
  #region Members
  private const string TIMESTAMP_DEFAULT_VALUE = "(strftime('%s','now') * 1000)";
  #endregion


  #region Constructor
  public SqliteDbContext(DbContextOptions<SqliteDbContext> options)
    : base(options) { }
  #endregion


  #region Properties
  public DbSet<EntityRepository> Entities { get; set; }
  #endregion


  #region Private methods
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<EntityRepository>(entity =>
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