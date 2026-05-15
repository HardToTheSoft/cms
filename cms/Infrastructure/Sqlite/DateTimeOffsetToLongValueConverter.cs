using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Cms.Infrastructure.Sqlite;


public class DateTimeOffsetToLongValueConverter : ValueConverter<DateTimeOffset, long>
{
  public DateTimeOffsetToLongValueConverter()
    : base(static dateTimeOffset => dateTimeOffset.ToUnixTimeMilliseconds(),
      static milliseconds => DateTimeOffset.FromUnixTimeMilliseconds(milliseconds))
  { }
}