namespace TollFeeCalculator.Vehicles;

/// <summary>
/// The implementation of a vehicle
/// </summary>
public class Vehicle(VehicleType typeOfVehicle) : IVehicle
{
    public VehicleType TypeOfVehicle { get; } = typeOfVehicle;
}