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

    /// <summary>
    /// Specifies if the vehicle is tollfree
    /// </summary>
    bool TollFree { get; }
}