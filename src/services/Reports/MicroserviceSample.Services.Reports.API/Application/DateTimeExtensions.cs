namespace MicroserviceSample.Services.Reports.API.Application
{
    public static class DateTimeExtensions
    {
        public static DateTime Now => DateTime.UtcNow;

        public static DateTime LocalNow =>
            TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time"));
    }
}
