namespace TollFeeCalculator;

public class TollPassage(DateTime timeOfPassage)
{
    public DateTime TimeOfPassage { get; } = timeOfPassage;
    public bool Processed { get; set; }
    public int Fee
    {
        get
        {
            // Extract the time part from the DateTime as a TimeOnly
            var time = TimeOnly.FromDateTime(TimeOfPassage); // This might cause an issue when it comes to milliseconds

            // Here I almost missed the milliseconds where the passage might be between 59 seconds and 00 seconds
            return time switch
            {
                _ when time >= new TimeOnly(06, 00, 00) && time < new TimeOnly(06, 30, 00) => 8,
                _ when time >= new TimeOnly(06, 30, 00) && time < new TimeOnly(07, 00, 00) => 13,
                _ when time >= new TimeOnly(07, 00, 00) && time < new TimeOnly(08, 00, 00) => 18,
                _ when time >= new TimeOnly(08, 00, 00) && time < new TimeOnly(08, 30, 00) => 13,
                _ when time >= new TimeOnly(08, 30, 00) && time < new TimeOnly(15, 00, 00) => 8,
                _ when time >= new TimeOnly(15, 00, 00) && time < new TimeOnly(15, 30, 00) => 13,
                _ when time >= new TimeOnly(15, 30, 00) && time < new TimeOnly(17, 00, 00) => 18,
                _ when time >= new TimeOnly(17, 00, 00) && time < new TimeOnly(18, 00, 00) => 13,
                _ when time >= new TimeOnly(18, 00, 00) && time < new TimeOnly(18, 30, 00) => 8,
                _ => 0
            };
        }

    }
}