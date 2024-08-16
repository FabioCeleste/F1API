namespace F1RestAPI.Contracts.DriversConstructors;

public record GetAllDriversConstructorsByParamsRequest(
        int? Page,
        int? Pagesize,
        int? Year,
        int? ConstructorId,
        int? DriverId
);