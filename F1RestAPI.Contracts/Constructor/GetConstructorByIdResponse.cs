using F1RestAPI.Contracts.DriversConstructors;

namespace F1RestAPI.Contracts.Constructors;
public record GetConstructorByIdResponse(
    int ConstructorId,
    string ConstructorRef,
    string Name,
    string Nationality,
    string Url,
    IEnumerable<DriverConstructorWithDriverDto>? DriverConstructors
);