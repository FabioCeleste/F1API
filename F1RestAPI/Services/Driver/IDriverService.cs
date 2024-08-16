using F1RestAPI.Models;
using ErrorOr;
using F1RestAPI.Contracts.Pagination;
using F1RestAPI.Contracts.Drivers;

namespace F1RestAPI.Services.Drivers;

public interface IDriverService
{
    Task<ErrorOr<Driver>> CreateDriverAsync(Driver driver);
    Task<ErrorOr<Driver>> GetDriverById(int id);
    Task<ErrorOr<PagedResult<Driver>>> GetAllDriversAsync(int pageNumber, int pageSize);
    Task<ErrorOr<PagedResult<Driver>>> GetAllDriversByParamsAsync(GetAllDriversByParamsRequest queryParams);
    Task<ErrorOr<Driver>> UpdateDriverAsync(int Id, UpdateDriverRequest driver);
    Task<ErrorOr<Deleted>> DeleteDriverAsync(int id);
}
