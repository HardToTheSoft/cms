using System.Text.Json;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


public class JsonDocumentToJsonStringValueConverter : ValueConverter<JsonDocument?, string?>
{
  #region Constructor
  public JsonDocumentToJsonStringValueConverter(bool isRequired)
    : base(jsonDocument => SafeSerialize(jsonDocument, isRequired)!,
      jsonString => SafeDeserialize(jsonString, isRequired))
  { }
  #endregion


  #region Private methods
  private static string? SafeSerialize(JsonDocument? jsonDocument, bool isRequired)
  {
    if (jsonDocument is null)
      return isRequired ? string.Empty : null;

    try
    {
      return JsonSerializer.Serialize(jsonDocument);
    }
    catch
    {
      return isRequired ? string.Empty : null;
    }
  }


  private static JsonDocument? SafeDeserialize(string? json, bool isRequired)
  {
    if (string.IsNullOrWhiteSpace(json))
      return isRequired ? JsonDocument.Parse("{}") : null;

    try
    {
      return JsonDocument.Parse(json);
    }
    catch
    {
      return isRequired ? JsonDocument.Parse("{}") : null;
    }
  }
  #endregion
}