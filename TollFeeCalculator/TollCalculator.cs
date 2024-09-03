using Nager.Date;
using TollFeeCalculator.Vehicles;

namespace TollFeeCalculator;

public static class TollCalculator
{
   public const int MaxFee = 60;
   public const int TollFreeMonth = 7;
   public static DayOfWeek[] TollFreeDays = [DayOfWeek.Saturday, DayOfWeek.Sunday];

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

      int totalFee = 0;

      foreach (var tollpassage in tollPassages)
      {
         if (tollpassage.Processed)
         {
            continue;
         }

         var intervalEnd = tollpassage.TimeOfPassage.AddMinutes(60);

         // As the passages are ordered, TakeWhile will only process tollPassages until it reaches the intervalEnd and does not process the rest.
         var intervalPassages = tollPassages
               .TakeWhile(p => p.TimeOfPassage <= intervalEnd)
               .ToList();

         var maxFeeInInterval = intervalPassages.Max(p => p.Fee);

         totalFee += maxFeeInInterval;

         foreach (var passage in intervalPassages)
            passage.Processed = true;
         
         // Early exit if the total fee exceeds the max fee
         if (totalFee >= MaxFee)
         {
            return MaxFee;
         }
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
      return TollFreeDays.Contains(date.DayOfWeek) || date.Month == TollFreeMonth ||
             DateSystem.IsPublicHoliday(date, CountryCode.SE) || DateSystem.IsPublicHoliday(date.AddDays(1), CountryCode.SE);
   }
}