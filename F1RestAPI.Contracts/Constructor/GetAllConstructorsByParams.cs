namespace F1RestAPI.Contracts.Constructors;

public record GetAllConstructorsByParamsRequest(
        int? Page,
        int? Pagesize,
        string? ConstructorRef,
        string? Name,
        string? Nationality,
        string? Url
);