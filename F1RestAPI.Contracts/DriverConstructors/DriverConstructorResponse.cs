using F1RestAPI.Contracts.Constructors;
using F1RestAPI.Contracts.Drivers;

namespace F1RestAPI.Contracts.DriversConstructors;

public record DriverConstructorResponse(
   int Year,
   int ConstructorId,
   int DriverId,
   DriverDto Driver,
   ConstructorDto Constructor
);