using System.Text.Json;
using System.Runtime.CompilerServices;


namespace Cms.Domain;


public record Entity : IEntity
{
  #region Constructors
  public Entity()
  { }


  public Entity(
    string id,
    int version = 1,
    bool published = false,
    bool disabled = false,
    JsonDocument? payloadJson = null,
    DateTimeOffset? createdAt = null,
    DateTimeOffset? updatedAt = null
    )
  {
    Id = string.IsNullOrWhiteSpace(id) ? throw new ArgumentException("Id cannot be empty.") : id.Trim().ToLower();
    Version = ValidateVersion(version);
    Published = published;
    Disabled = disabled;
    PayloadJson = payloadJson;
    CreatedAt = createdAt ?? DateTimeOffset.UtcNow;
    UpdatedAt = updatedAt ?? DateTimeOffset.UtcNow;
  }
  #endregion


  #region Properties
  public string Id { get; } = default!;

  public int Version { get; private set; } = 1;

  public bool Published { get; }
  public bool Disabled { get; private set; }

  public JsonDocument? PayloadJson { get; private set; }

  public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
  public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;
  #endregion


  #region Public methods
  public void Publish(int version, JsonDocument? payloadJson)
  {
    SetVersion(version);

    PayloadJson = payloadJson;
  }

  public void Unpublish(int version, JsonDocument? payloadJson)
  {
    SetVersion(version);

    PayloadJson = payloadJson;
  }


  public void Disable()
  {
    Disabled = true;
    UpdatedAt = DateTimeOffset.UtcNow;
  }
  #endregion


  #region Private methods
  private void SetVersion(int version)
  {
    version = ValidateVersion(version);

    Version = version <= Version ? throw new ArgumentException("Invalid version.") : version;
  }


  [MethodImpl(MethodImplOptions.AggressiveInlining)]
  private static int ValidateVersion(int version)
    => version < 1 ? throw new ArgumentException("Invalid version.") : version;
  #endregion
}