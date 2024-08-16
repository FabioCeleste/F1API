namespace F1RestAPI.Contracts.Drivers;

public record GetAllDriversByParamsRequest(
        int? Page,
        int? Pagesize,
        string? DriverRef,
        int? DriverNumber,
        string? DriverCode,
        string? DriverForename,
        string? DriverSurname,
        DateTime? DateOfBirth,
        string? Nationality,
        string? WikipediaUrl,
        int? PolePositions,
        int? Wins,
        int? Podiumns,
        int? DNF
    );