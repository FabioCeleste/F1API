using F1RestAPI.Models;
using ErrorOr;
using F1RestAPI.Contracts.Pagination;
using F1RestAPI.Contracts.Constructors;

namespace F1RestAPI.Services.Constructors;

public interface IConstructorService
{
    Task<ErrorOr<Constructor>> CreateConstructorAsync(Constructor constructor);
    Task<ErrorOr<Constructor>> GetConstructorById(int id);
    Task<ErrorOr<PagedResult<Constructor>>> GetAllConstructorsAsync(int pageNumber, int pageSize);
    Task<ErrorOr<PagedResult<Constructor>>> GetAllConstructorsByParamsAsync(GetAllConstructorsByParamsRequest queryParams); // TODO
    Task<ErrorOr<Constructor>> UpdateConstructorAsync(int id, UpdateConstructorRequest constructor);
    Task<ErrorOr<Deleted>> DeleteConstructorAsync(int id);
}
