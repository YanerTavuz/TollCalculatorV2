# This solution test is to show my skills in .NET development

## Requirements
1. .NET 8 SDK

---

## Testing

To test the functionality of the code, run all tests in the testproject in Visual Studio or by running command ```dotnet test```

---

## Insights

* By using the new types TimeOnly and DateOnly in .NET 6 and above, it simplifies compares of times and dates. Working with datetimes is always a bit tricky, but with the help of these new types it was simpler and more clean.
* Removed the files Car.cs and Motorbike.cs, since it seemed better to have one **Vehicle** type with a property of VehicleType enum.
* There is always a thought about using a static class for calculations or implement methods in a class such as **Vehicle**. For me it's more of a flavor thing, and maybe another type of vehicle can be implemented that can use the same methods.
