namespace Cms.Services;


public interface IEvent
{
	public string RelayPlatform { get; set; }
	public string RelayHandle { get; set; }
	public string RelayExternalId { get; set; }

	public object? Id { get; set; }

	public string? Topic { get; set; }

	public DateTimeOffset TriggeredAt { get; set; }

	public object? Payload { get; set; }

	public string? PayloadType { get; set; }

	string? Hash { get; }

	string? ComputeHash();
}