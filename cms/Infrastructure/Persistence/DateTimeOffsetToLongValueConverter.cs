using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace Cms.Infrastructure.Persistence;


public class DateTimeOffsetToLongValueConverter : ValueConverter<DateTimeOffset?, long?>
{
  #region Members
  private static long _minUnixTimeMilliseconds = DateTimeOffset.MinValue.ToUnixTimeMilliseconds();
  private static long _maxUnixTimeMilliseconds = DateTimeOffset.MaxValue.ToUnixTimeMilliseconds();
  #endregion


  #region Constructor
  public DateTimeOffsetToLongValueConverter(bool isRequired)
    : base(dateTimeOffset => SafeSerialize(dateTimeOffset, isRequired),
      milliseconds => SafeDeserialize(milliseconds, isRequired))
  { }
  #endregion


  #region Private methods
  private static long? SafeSerialize(DateTimeOffset? dateTimeOffset, bool isRequired)
  {
    if (dateTimeOffset is null)
      return isRequired ? DateTimeOffset.MinValue.ToUnixTimeMilliseconds() : null;

    try
    {
      return dateTimeOffset.Value.ToUnixTimeMilliseconds();
    }
    catch
    {
      return isRequired ? DateTimeOffset.MinValue.ToUnixTimeMilliseconds() : null;
    }
  }


  private static DateTimeOffset? SafeDeserialize(long? milliseconds, bool isRequired)
  {
    if (milliseconds is null || !milliseconds.HasValue
      || milliseconds < _minUnixTimeMilliseconds || milliseconds > _maxUnixTimeMilliseconds)
      return isRequired ? DateTimeOffset.MinValue : null;

    return DateTimeOffset.FromUnixTimeMilliseconds(milliseconds.Value);
  }
  #endregion
}