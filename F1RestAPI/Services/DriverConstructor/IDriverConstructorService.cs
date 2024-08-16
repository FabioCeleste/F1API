using F1RestAPI.Models;
using ErrorOr;
using F1RestAPI.Contracts.Pagination;
using F1RestAPI.Contracts.DriversConstructors;

namespace F1RestAPI.Services.DriversConstructors;

public interface IDriverConstructorService
{
    Task<ErrorOr<DriverConstructor>> CreateDriverConstructorAsync(DriverConstructor driverConstructor);
    Task<ErrorOr<DriverConstructor>> GetDriverConstructorByIdAsync(int year, int driverId, int constructorId);
    Task<ErrorOr<PagedResult<DriverConstructor>>> GetAllDriversConstructorsAsync(int pageNumber, int pageSize);
    Task<ErrorOr<PagedResult<DriverConstructor>>> GetAllDriversConstructorsByParamsAsync(GetAllDriversConstructorsByParamsRequest queryParams);
    Task<ErrorOr<DriverConstructor>> UpdateDriverConstructorAsync(UpdateDriverConstructorRequest queryParams, UpdateDriverConstructorRequest bodyParams);
    Task<ErrorOr<Deleted>> DeleteDriverConstructorAsync(DeleteDriverConstructorRequest queryParams);
}
