namespace Unisphere.Core.Common.Extensions;

public static class DateOnlyExtensions
{
    public static DateTime ToDateTime(this DateOnly date)
    {
        return date.ToDateTime(TimeOnly.MinValue);
    }

    public static DateOnly? FirstDayOfMonth(this DateOnly? date)
    {
        if (date == null)
        {
            return null;
        }

        return date.Value.FirstDayOfMonth();
    }

    public static DateOnly FirstDayOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    public static DateOnly? LastDayOfMonth(this DateOnly? date)
    {
        if (date == null)
        {
            return null;
        }

        return date.Value.LastDayOfMonth();
    }

    public static DateOnly LastDayOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
    }
}
