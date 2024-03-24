using Quartz;

namespace BackupUtilities.Shared;

public static class Cron
{
    /// <summary>
    /// Fix a cron expression to be valid.
    ///
    /// I blame Quartz for this...
    /// </summary>
    /// <param name="cron">The cron expression to fix</param>
    /// <returns>The fixed cron expression</returns>
    public static string Fix(string cron)
    {
        // Split the cron expression into parts
        List<string> parts = [..cron.Split(' ')];

        // If there are less than 6 parts, add a 0 to the start (seconds)
        if (parts.Count < 6)
            parts.Insert(0, "0");

        // If day of month / week is * and the other is not, set it to ?
        if (parts[3] == "*" && parts[5] == "*" || parts[3] != "*" && parts[5] == "*")
            parts[5] = "?";
        else if (parts[3] == "*" && parts[5] != "*")
            parts[3] = "?";

        // Return the fixed cron expression
        return string.Join(' ', parts);
    }

    /// <summary>
    /// Validate a cron expression.
    /// </summary>
    /// <param name="cron">The cron expression to validate</param>
    /// <returns>Whether the cron expression is valid</returns>
    public static bool Validate(string cron)
    {
        try
        {
            _ = new CronExpression(Fix(cron));
            return true;
        }
        catch
        {
            return false;
        }
    }
}
