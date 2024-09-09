namespace TollFeeCalculator;

public class TollPassage
{
    public DateTime TimeOfPassage { get; }
    public bool Processed { get; private set; }
    public int Fee { get; }

    public TollPassage(DateTime timeOfPassage)
    {
        // Set properties once to avoid calculating value each time property is accessed 
        TimeOfPassage = timeOfPassage;
        Fee = CalculateFee();
    }

    public void SetToProcessed()
    {
        Processed = true;
    }
    
    private int CalculateFee()
    {
        // Extract the time part from the DateTime as a TimeOnly
        var time = TimeOnly.FromDateTime(TimeOfPassage);

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