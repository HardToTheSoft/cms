using System.Text;
using System.Reflection;
using System.Security.Cryptography;

using Cms.Application.Interfaces;
using Cms.Infrastructure.Attributes;


namespace Cms.Application.Models;


public abstract class EventAbstract<TEvent> : IEvent, IEventHandler<TEvent>
	where TEvent : class, IEvent, IEventHandler<TEvent>, new()
{
	#region Members
	private const string NULL = "null";

	private string? _hash = null;
	#endregion


	#region Properties
	[Hash]
	public string RelayPlatform { get; set; } = default!;

	[Hash]
	public string RelayHandle { get; set; } = default!;

	[Hash]
	public string RelayExternalId { get; set; } = default!;

	[Hash]
	public object? Id { get; set; }

	[Hash]
	public string? Topic { get; set; }

	public DateTimeOffset TriggeredAt { get; set; } = DateTimeOffset.MinValue;

	public object? Payload { get; set; }

	public string? PayloadType { get; set; }

	public string? Hash => _hash ?? ComputeHash();

	public Func<TEvent, Task>? EventHandler { get; set; }
	#endregion


	#region Public methods
	public string? ComputeHash()
	{
		_hash = null;

		var properties = GetType()
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(pi => pi.CanRead && Attribute.IsDefined(pi, typeof(HashAttribute), false))
				.OrderBy(pi => pi.Name)
				.ToArray();

		if (properties.Length == 0)
			return _hash;

		var builder = new StringBuilder();

		foreach (var propertyInfo in properties)
			builder.Append($"{propertyInfo.Name}:{propertyInfo.GetValue(this)?.ToString() ?? NULL};");

		byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(builder.ToString()));

		_hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

		return _hash;
	}
	#endregion
}