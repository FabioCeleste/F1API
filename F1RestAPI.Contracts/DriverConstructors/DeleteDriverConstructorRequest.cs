namespace F1RestAPI.Contracts.DriversConstructors;

public record DeleteDriverConstructorRequest(
        int Year,
        int ConstructorId,
        int DriverId
);