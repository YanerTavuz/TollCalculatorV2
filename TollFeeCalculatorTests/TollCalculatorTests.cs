using TollFeeCalculator;
using TollFeeCalculator.Vehicles;
using Xunit;

namespace TollFeeCalculatorTests;

public class TollCalculatorTests
{
    [Fact]
    public void GetTollFee_IntervalOfTimes_ReturnsCorrectFee()
    {
        DateTime[] dates =
        [
            // First interval will give 16 as fee
            new DateTime(2021, 01, 08, 06, 45, 00),
            new DateTime(2021, 01, 08, 06, 10, 00),
            new DateTime(2021, 01, 08, 06, 15, 00),
            new DateTime(2021, 01, 08, 06, 35, 00),

            // Second interval will give 16 as fee
            new DateTime(2021, 01, 08, 08, 25, 00),
            new DateTime(2021, 01, 08, 08, 35, 00),
            new DateTime(2021, 01, 08, 08, 05, 00),

            // Third interval will give 16 as fee
            new DateTime(2021, 01, 08, 14, 50, 00),
            new DateTime(2021, 01, 08, 15, 15, 00),
            new DateTime(2021, 01, 08, 15, 32, 00)
        ];

        var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates);

        Assert.Equal(44, actual);
    }

    [Fact]
    public void GetTollFee_MaxFeeReached_ReturnsCorrectFee()
    {
        DateTime[] dates =
        [
            // No intervals will actual in each entry counted
            new DateTime(2021, 01, 08, 06, 45, 00),
            new DateTime(2021, 01, 08, 08, 05, 00),
            new DateTime(2021, 01, 08, 11, 05, 00),
            new DateTime(2021, 01, 08, 15, 15, 00),
            new DateTime(2021, 01, 08, 16, 45, 00)
        ];

        var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates);

        Assert.Equal(TollCalculator.MaxFee, actual);
    }

    [Fact]
    public void GetTollFee_OnePassage_ReturnsCorrectFee()
    {
        DateTime[] dates = [new DateTime(2021, 01, 08, 06, 10, 00)];

        var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates);

        Assert.Equal(8, actual);
    }

    [Fact]
    public void GetTollFee_OnePassageWithSecondsInDateTime_ReturnsCorrectFee()
    {
        DateTime[] dates = [new DateTime(2021, 01, 08, 06, 29, 59)];

        var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates);

        Assert.Equal(8, actual);
    }

    [Fact]
    public void GetTollFee_TollFreeVehicle_Returns0()
    {
        DateTime[] dates =
        [
            new DateTime(2021, 01, 08, 06, 10, 00),
            new DateTime(2021, 01, 08, 06, 45, 00),
            new DateTime(2021, 01, 08, 06, 15, 00),
            new DateTime(2021, 01, 08, 15, 35, 00)
        ];

        var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Motorbike), dates);

        Assert.Equal(0, actual);
    }

    [Fact]
    public void GetTollFee_Sunday_Returns0()
    {
        DateTime[] dates =
        [
            new DateTime(2021, 01, 10, 06, 10, 00)
        ];

        var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates);

        Assert.Equal(0, actual);
    }

    [Fact]
    public void GetTollFee_Saturday_Returns0()
    {
        DateTime[] dates =
        [
            new DateTime(2021, 01, 09, 06, 10, 00)
        ];

        var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates);

        Assert.Equal(0, actual);
    }

    [Fact]
    public void GetTollFee_SwedishHolidayOnAWeekday_Returns0()
    {
        DateTime[] dates =
        [
            new DateTime(2018, 06, 06, 06, 10, 00) // Sixth of June
        ];

        var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates);

        Assert.Equal(0, actual);
    }

    [Fact]
    public void GetTollFee_DayBeforeSwedishHolidayOnAWeekday_Returns0()
    {
        DateTime[] dates =
        [
            new DateTime(2018, 06, 05, 06, 10, 00) // The day before a holiday is toll-free
        ];

        var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates);

        Assert.Equal(0, actual);
    }

    [Fact]
    public void GetTollFee_JulyDifferentYears_Returns0()
    {
        for (var year = 2005; year < 2021; year++)
        {
            for (var day = 1; day < 31; day++)
            {
                DateTime[] dates =
                [
                    new DateTime(year, TollCalculator.TollFreeMonth, day, 06, 10, 00)
                ];
                var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates);

                Assert.Equal(0, actual);
            }
        }
    }

    [Fact]
    public void GetTollFee_NoDates_Returns0()
    {
        DateTime[] dates = [];

        var actual = TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates);

        Assert.Equal(0, actual);
    }

    [Fact]
    public void GetTollFee_DifferentDates_Throws()
    {
        DateTime[] dates =
        [
            new DateTime(2018, 06, 01, 01, 10, 00),
            new DateTime(2018, 06, 02, 02, 10, 00)
        ];

        
        Assert.Throws<ArgumentException>(() => TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), dates));
    }

    [Fact]
    public void GetTollFee_VehicleIsNull_Throws()
    {
        DateTime[] dates =
        [
            new DateTime(2018, 06, 01, 01, 10, 00)
        ];

        Assert.Throws<ArgumentNullException>(() => TollCalculator.GetTollFee(null, dates));
    }

    [Fact]
    public void GetTollFee_DatesIsNull_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => TollCalculator.GetTollFee(new Vehicle(VehicleType.Car), null));
    }

}