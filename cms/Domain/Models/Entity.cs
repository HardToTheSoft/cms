using System.Text.Json;


namespace Cms.Domain.Models;


public sealed class EntityEntity
{
  public string Id { get; init; } = default!;

  public int Version { get; init; }

  public bool Published { get; init; }
  public bool Disabled { get; init; }

  public JsonElement Payload { get; init; }

  public DateTimeOffset CreatedAt { get; init; }
  public DateTimeOffset UpdatedAt { get; init; }
}