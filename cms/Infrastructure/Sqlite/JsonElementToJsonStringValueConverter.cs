using System.Text.Json;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Cms.Infrastructure.Sqlite;


public class JObjectToJsonStringValueConverter : ValueConverter<JsonElement, string>
{
  public JObjectToJsonStringValueConverter()
    : base(static expandoObject => expandoObject.ToString(),
#pragma warning disable CS8603 // Possible null reference return.
    static jsonString => JsonElement.Parse(jsonString))
#pragma warning restore CS8603 // Possible null reference return.
  { }
}