namespace Cms.Data;


public sealed class Entity : IEntity
{
  public int Id { get; set; }

  public bool Published { get; set; }

  public bool Deleted { get; set; }

  public string? PayloadJson { get; set; }

  public int Version { get; set; }

  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}