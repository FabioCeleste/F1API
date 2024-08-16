namespace F1RestAPI.Contracts.DriversConstructors;

public record UpdateDriverConstructorRequest(
        int? Year,
        int? ConstructorId,
        int? DriverId
);