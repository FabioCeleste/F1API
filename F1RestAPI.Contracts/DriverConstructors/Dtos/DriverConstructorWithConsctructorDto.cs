using F1RestAPI.Contracts.Constructors;

namespace F1RestAPI.Contracts.DriversConstructors;
public record DriverConstructorWithConsctructorDto(
   int Year,
   int ConstructorId,
   int DriverId,
   ConstructorDto Constructor
);
