using System.Text.Json;


namespace Cms.Application.Dto;


public sealed class AdminEntityDto
{
  public string Id { get; set; } = default!;

  public int Version { get; set; }

  public bool Published { get; set; }
  public bool Disabled { get; set; }

  public JsonDocument PayloadJson { get; set; } = default!;

  public DateTimeOffset CreatedAt { get; set; }
}