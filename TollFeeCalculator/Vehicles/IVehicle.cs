namespace TollFeeCalculator.Vehicles;

/// <summary>
/// Interface to be used by all vehicle classes.
/// </summary>
public interface IVehicle
{
    /// <summary>
    /// Specifies which type of vehicle it is
    /// </summary>
    VehicleType TypeOfVehicle { get; }
}