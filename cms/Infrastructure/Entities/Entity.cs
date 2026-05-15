using System.Text.Json;


namespace Cms.Infrastructure.Entities;


public sealed class Entity : IEntity
{
  public string Id { get; set; } = default!;

  public int Version { get; set; }

  public bool Published { get; set; }
  public bool Disabled { get; set; }

  public JsonElement Payload { get; set; }

  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}