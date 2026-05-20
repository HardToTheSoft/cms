using System.Text.Json;


namespace Cms.Application.Dto;


public sealed class UserEntityDto
{
  public string Id { get; set; } = default!;

  public int Version { get; set; }

  public bool Published { get; set; }

  public JsonDocument PayloadJson { get; set; } = default!;

  public DateTimeOffset CreatedAt { get; set; }
}