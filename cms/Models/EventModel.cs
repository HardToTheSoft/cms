using System.Text.Json;


namespace Cms.Models;


public sealed class EventModel
{
  public string Type { get; set; }
  public string Id { get; set; }
  public int? Version { get; set; }
  public JsonElement? Payload { get; set; }
  public DateTime Timestamp { get; set; }
}