using F1RestAPI.Models;
using ErrorOr;
using F1RestAPI.Data;
using Microsoft.EntityFrameworkCore;
using F1RestAPI.Contracts.Constructors;
using F1RestAPI.Contracts.Pagination;

namespace F1RestAPI.Services.Constructors;

public class ConstructorService : IConstructorService
{
    private readonly DataContext _dataContext;

    public ConstructorService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<ErrorOr<Constructor>> GetConstructorById(int id)
    {
        try
        {
            var constructor = await _dataContext.Constructors.Include(c => c.DriverConstructors).ThenInclude(dC => dC.Driver).FirstOrDefaultAsync(c => c.ConstructorId == id);

            if (constructor == null)
            {
                return Error.NotFound("Constructor not found");
            }

            constructor.DriverConstructors = constructor.DriverConstructors.OrderByDescending(dC => dC.Year);
            return constructor;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}"); ;
        }

    }

    public async Task<ErrorOr<PagedResult<Constructor>>> GetAllConstructorsAsync(int pageNumber, int pageSize)
    {
        try
        {
            var pageSizeWithMaxLimit50 = pageSize;
            if (pageSizeWithMaxLimit50 > 50)
            {
                pageSizeWithMaxLimit50 = 50;
            }

            var totalCount = await _dataContext.Constructors.CountAsync();

            var constructors = await _dataContext.Constructors.OrderBy(c => c.ConstructorId).Skip((pageNumber - 1) * pageSizeWithMaxLimit50)
            .Take(pageSizeWithMaxLimit50).ToListAsync();

            if (constructors == null || constructors.Count == 0)
            {
                return Error.NotFound("Constructors not found");
            }

            var pagedResult = new PagedResult<Constructor>(constructors, totalCount);

            return pagedResult;
        }
        catch (Exception ex)
        {
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<Constructor>>> GetAllConstructorsByParamsAsync(GetAllConstructorsByParamsRequest queryParams)
    {
        try
        {
            var query = _dataContext.Constructors.AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.Nationality))
            {
                query = query.Where(d => d.Nationality == queryParams.Nationality);
            }

            if (!string.IsNullOrEmpty(queryParams.Name))
            {
                query = query.Where(d => d.Name == queryParams.Name);
            }

            if (!string.IsNullOrEmpty(queryParams.ConstructorRef))
            {
                query = query.Where(d => d.ConstructorRef == queryParams.ConstructorRef);
            }

            if (!string.IsNullOrEmpty(queryParams.Url))
            {
                query = query.Where(d => d.Url == queryParams.Url);
            }

            int totalCount = await query.CountAsync();

            var pageSizeWithMaxLimit50 = queryParams.Pagesize ?? 15;
            if (pageSizeWithMaxLimit50 > 50)
            {
                pageSizeWithMaxLimit50 = 50;
            }


            int skip = ((queryParams.Page ?? 1) - 1) * pageSizeWithMaxLimit50;
            query = query.Skip(skip).Take(pageSizeWithMaxLimit50);

            var constructors = await query.ToListAsync();

            var pagedResult = new PagedResult<Constructor>(constructors, totalCount);

            return pagedResult;
        }
        catch (Exception ex)
        {
            return Error.Failure(description: ex.Message);
        }

    }

    public async Task<ErrorOr<Constructor>> UpdateConstructorAsync(int id, UpdateConstructorRequest updatedConstructor)
    {
        try
        {
            var existingConstructor = await _dataContext.Constructors.FindAsync(id);

            if (existingConstructor == null)
            {
                return Error.NotFound("Constructors not found");
            }

            existingConstructor.Nationality = updatedConstructor.Nationality ?? existingConstructor.Nationality;
            existingConstructor.Url = updatedConstructor.Url ?? existingConstructor.Url;
            existingConstructor.Name = updatedConstructor.Name ?? existingConstructor.Name;
            existingConstructor.ConstructorRef = updatedConstructor.ConstructorRef ?? existingConstructor.ConstructorRef;

            await _dataContext.SaveChangesAsync();

            return existingConstructor;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ErrorOr<Constructor>> CreateConstructorAsync(Constructor constructor)
    {
        try
        {
            _dataContext.Constructors.Add(constructor);
            await _dataContext.SaveChangesAsync();
            return constructor;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ErrorOr<Deleted>> DeleteConstructorAsync(int id)
    {
        try
        {
            var constructor = await _dataContext.Constructors.FindAsync(id);
            if (constructor == null)
            {
                return Error.NotFound("Constructor not found");
            }
            _dataContext.Constructors.Remove(constructor);
            await _dataContext.SaveChangesAsync();
            return new Deleted();
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }
    }
}
