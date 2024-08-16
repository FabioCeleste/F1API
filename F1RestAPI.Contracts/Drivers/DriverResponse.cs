using F1RestAPI.Contracts.DriversConstructors;

namespace F1RestAPI.Contracts.Drivers;

public record DriverResponse(
    int DriverId,
    string DriverRef,
    int DriverNumber,
    string DriverCode,
    string DriverForename,
    string DriverSurname,
    DateTime DateOfBirth,
    string Nationality,
    string WikipediaUrl,
    int PolePositions,
    int Wins,
    int Podiumns,
    int DNF
);