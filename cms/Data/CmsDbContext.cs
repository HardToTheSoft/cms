using Microsoft.EntityFrameworkCore;

using Cms.Models;


namespace Cms.Data;


public sealed class CmsDbContext : DbContext
{
  #region Constructor
  public CmsDbContext(DbContextOptions<CmsDbContext> options)
    : base(options) { }
  #endregion


  #region Properties
  public DbSet<EntityModel> Entities { get; set; }
  #endregion
}