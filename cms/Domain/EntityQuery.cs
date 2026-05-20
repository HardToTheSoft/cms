namespace Cms.Domain;


public sealed class EntityQuery
{
  public string Id { get; set; } = default!;

  public bool Published { get; set; }
}