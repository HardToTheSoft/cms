using System.Text.Json;

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


public class JsonDocumentToJsonStringValueConverter : ValueConverter<JsonDocument, string>
{
  #region Constructor
  public JsonDocumentToJsonStringValueConverter()
    : base(static jsonDocument => JsonSerializer.Serialize(jsonDocument),
      static jsonString => SafeParse(jsonString))
  { }
  #endregion


  #region Private methods
  private static JsonDocument SafeParse(string jsonString)
  {
    if (string.IsNullOrWhiteSpace(jsonString))
      return JsonDocument.Parse("{}");

    try
    {
      return JsonDocument.Parse(jsonString);
    }
    catch (JsonException)
    {
      return JsonDocument.Parse("{}");
    }
  }
  #endregion
}