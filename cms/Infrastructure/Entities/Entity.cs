namespace Cms.Data;


public sealed class Entity : IEntity
{
  public string Id { get; set; }

  public int Version { get; set; }

  public bool Published { get; set; }
  public bool Disabled { get; set; }

  public string? PayloadJson { get; set; }

  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}