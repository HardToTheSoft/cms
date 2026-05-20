using System.Text.Json;


namespace Cms.Application.Models;


public sealed class EventModel
{
  public string Id { get; set; } = default!;
  public string Type { get; set; } = default!;

  public int? Version { get; set; }

  public JsonDocument? PayloadJson { get; set; }

  public DateTimeOffset Timestamp { get; set; }
}