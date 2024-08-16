namespace F1RestAPI.Contracts.Drivers;

public record GetAllDriversRequest(
 int? Page,
 int? Pagesize
);