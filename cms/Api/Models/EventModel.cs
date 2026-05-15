using System.Text.Json;


namespace Cms.Models;


public sealed class EventModel
{
  public string Id { get; set; } = default!;
  public string Type { get; set; } = default!;

  public int? Version { get; set; }

  public JsonElement? Payload { get; set; }
  
  public DateTimeOffset Timestamp { get; set; }
}