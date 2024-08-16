namespace F1RestAPI.Contracts.DriversConstructors;

public record CreateDriverConstructorRequest(
   int Year,
   int ConstructorId,
   int DriverId
);