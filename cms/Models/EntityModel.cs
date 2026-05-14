using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Cms.Models;


[Table("Entities")]
public sealed class EntityModel
{
  [Key]
  public string Id { get; set; }

  public int CurrentVersion { get; set; }

  public bool Published { get; set; }

  public bool Deleted { get; set; }

  public string PayloadJson { get; set; }

  public DateTime LastUpdated { get; set; }
}