namespace InitialSetupBackend.Shared.Helpers
{
    public static class DateHelper
    {
        public static DateTime GetBrazilianTime()
        {
            string brasiliaTimeZoneId = OperatingSystem.IsWindows()
                                        ? "E. South America Standard Time"
                                        : "America/Sao_Paulo";

            TimeZoneInfo brasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById(brasiliaTimeZoneId);

            DateTime utcNow = DateTime.UtcNow;

            DateTime brasiliaTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, brasiliaTimeZone);

            return DateTime.SpecifyKind(brasiliaTime, DateTimeKind.Unspecified);
        }

        public static bool IsDate(string date)
        {
            return DateTime.TryParse(date, out _);
        }
    }
}
