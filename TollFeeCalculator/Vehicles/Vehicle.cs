namespace TollFeeCalculator.Vehicles;

/// <summary>
/// The implementation of a vehicle
/// </summary>
public class Vehicle : IVehicle
{
   public VehicleType TypeOfVehicle { get; }

   public bool TollFree { get; }
   
   public Vehicle(VehicleType typeOfVehicle)
   {
      // Set properties once to avoid calculating value each time property is accessed 
      TypeOfVehicle = typeOfVehicle;
      TollFree = TypeOfVehicle switch
      {
         VehicleType.Motorbike or VehicleType.Tractor or VehicleType.Emergency or VehicleType.Diplomat or VehicleType.Foreign or VehicleType.Military => true,
         _ => false
      };
   }
}