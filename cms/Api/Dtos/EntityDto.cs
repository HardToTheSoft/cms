using System.Text.Json;


namespace Cms.Api.Dto;


public sealed class EntityDto
{
  public string Id { get; set; } = default!;

  public int Version { get; set; }

  public bool Published { get; set; }
  public bool Disabled { get; set; }

  public JsonElement PayloadJson { get; set; }

  public DateTimeOffset CreatedAt { get; set; }
}