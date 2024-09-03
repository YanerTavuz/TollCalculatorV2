using Nager.Date;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public static class TollCalculator
{
    public const int MaxFee = 60;
    public const int TollFreeMonth = 7;

    /// <summary>
    /// Calculate the total toll fee for one day based on vehicle type and passage dates.
    /// </summary>
    /// <param name="vehicle">The vehicle.</param>
    /// <param name="dates">Date and time of all passes on one day.</param>
    /// <returns>The total toll fee for that day.</returns>
    public static int GetTollFee(IVehicle vehicle, DateTime[] dates)
    {
        if (dates == null || dates.Length == 0)
           throw new ArgumentNullException(nameof(dates), "Dates were null or empty.");

        if (vehicle is null)
           throw new ArgumentNullException(nameof(vehicle), "Vehicle was null.");

        if (dates.Select(d => d.Date).Distinct().Count() > 1)
           throw new ArgumentException("All dates must be on the same day.", nameof(dates));

        if (vehicle.TollFree || IsTollFreeDate(dates[0]))
           return 0;

        var totalFee = CalculateTotalFee(dates);
        return Math.Min(totalFee, MaxFee); // Ensure the fee does not exceed the max fee.
    }

    /// <summary>
    /// Calculate the total toll fee for all toll passages in one day.
    /// </summary>
    /// <param name="dates">The dates and times of all passes.</param>
    /// <returns>The total fee for the day.</returns>
    private static int CalculateTotalFee(DateTime[] dates)
    {
        var tollPassages = dates
            .Select(d => new TollPassage(d))
            .OrderBy(p => p.TimeOfPassage)
            .ToList();

        var totalFee = 0;

        for (var i = 0; i < tollPassages.Count; i++)
        {
            var currentPassage = tollPassages[i];
            if (currentPassage.Processed)
                continue;

            // Define the interval end as 60 minutes from the current passage.
            var intervalEnd = currentPassage.TimeOfPassage.AddMinutes(60);

            // Find all passages within the next 60 minutes using index range
            var j = i;
            while (j < tollPassages.Count && tollPassages[j].TimeOfPassage <= intervalEnd)
            {
                j++;
            }

            // Get the maximum fee within the range of passages [i..j]
            var maxFeeInInterval = tollPassages[i..j].Max(p => p.Fee);

            // Add the maximum fee to the total fee
            totalFee += maxFeeInInterval;

            // Mark the passages as processed in the interval [i..j]
            for (var k = i; k < j; k++)
            {
                tollPassages[k].Processed = true;
            }

            // Skip the already processed elements by advancing i to j - 1
            i = j - 1;

            // Stop early if total fee exceeds the maximum
            if (totalFee >= MaxFee)
                return MaxFee;
        }

        return totalFee;
    }

    /// <summary>
    /// Checks if the date is toll-free.
    /// </summary>
    /// <param name="date">The date to check.</param>
    /// <returns><see langword="true"/> if the date is toll-free, otherwise <see langword="false"/>.</returns>
    private static bool IsTollFreeDate(DateTime date)
    {
        // Check if the date is a Saturday or Sunday, a public holiday, the day before a public holiday or in the toll-free month
        return date.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday || date.Month == TollFreeMonth ||
               DateSystem.IsPublicHoliday(date, CountryCode.SE) || DateSystem.IsPublicHoliday(date.AddDays(1), CountryCode.SE);
    }
}