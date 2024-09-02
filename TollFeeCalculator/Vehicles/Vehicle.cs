namespace TollFeeCalculator.Vehicles;

/// <summary>
/// The implementation of a vehicle
/// </summary>
public class Vehicle(VehicleType typeOfVehicle) : IVehicle
{
    public VehicleType TypeOfVehicle { get; } = typeOfVehicle;

    public bool TollFree 
    {
        get
        {
            return TypeOfVehicle switch
            {
                VehicleType.Motorbike or VehicleType.Tractor or VehicleType.Emergency or VehicleType.Diplomat or VehicleType.Foreign or VehicleType.Military => true,
                _ => false
            };
        }
    }         
}