namespace F1RestAPI.Contracts.Constructors;

public record ConstructorDto(
    int ConstructorId,
    string ConstructorRef,
    string Name,
    string Nationality,
    string Url
);