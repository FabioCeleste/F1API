using F1RestAPI.Contracts.DriversConstructors;

namespace F1RestAPI.Contracts.Constructor;

public record CreateConstructorRequest(
    string ConstructorRef,
    string Name,
    string Nationality,
    string Url
);
