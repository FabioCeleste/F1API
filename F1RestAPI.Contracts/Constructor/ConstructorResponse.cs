using F1RestAPI.Contracts.DriversConstructors;

namespace F1RestAPI.Contracts.Constructor;

public record ConstructorResponse(
    int ConstructorId,
    string ConstructorRef,
    string Name,
    string Nationality,
    string Url
    );
