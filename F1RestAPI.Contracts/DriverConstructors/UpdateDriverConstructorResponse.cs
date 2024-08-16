namespace F1RestAPI.Contracts.DriversConstructors;

public record UpdateDriverConstructorResponse(
        int Year,
        int ConstructorId,
        int DriverId
);