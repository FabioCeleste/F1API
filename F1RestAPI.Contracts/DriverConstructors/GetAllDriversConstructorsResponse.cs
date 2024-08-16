using F1RestAPI.Contracts.Constructors;
using F1RestAPI.Contracts.Drivers;

namespace F1RestAPI.Contracts.DriversConstructors;

public record GetAllDriversConstructorsResponse(
        int Year,
        int ConstructorId,
        int DriverId,
        DriverDto driver,
        ConstructorDto constructor
);