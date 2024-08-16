namespace F1RestAPI.Contracts.Constructors;
public record UpdateConstructorRequest(
    string? ConstructorRef,
    string? Name,
    string? Nationality,
    string? Url
   );
