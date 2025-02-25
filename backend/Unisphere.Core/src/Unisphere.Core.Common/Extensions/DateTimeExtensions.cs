namespace Unisphere.Core.Common.Extensions;

public static class DateTimeExtensions
{
    public static DateTime? FirstDayOfMonth(this DateTime? date)
    {
        if (date == null)
        {
            return null;
        }

        return FirstDayOfMonth(date.Value);
    }

    public static DateTime FirstDayOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Unspecified);
    }

    public static DateTime? LastDayOfMonth(this DateTime? date)
    {
        if (date == null)
        {
            return null;
        }

        return LastDayOfMonth(date.Value);
    }

    public static DateTime LastDayOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1, 0, 0, 0, DateTimeKind.Unspecified).AddMonths(1).AddDays(-1);
    }
}
