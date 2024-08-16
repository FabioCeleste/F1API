using F1RestAPI.Models;
using ErrorOr;
using F1RestAPI.Data;
using F1RestAPI.Contracts.DriversConstructors;
using Microsoft.EntityFrameworkCore;
using F1RestAPI.Contracts.Pagination;

namespace F1RestAPI.Services.DriversConstructors;

public class DriverConstructorService : IDriverConstructorService
{
    private readonly DataContext _dataContext;

    public DriverConstructorService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<ErrorOr<DriverConstructor>> GetDriverConstructorByIdAsync(int year, int driverId, int constructorId)
    {
        var driverConstructor = await _dataContext.DriverConstructors.Include(dC => dC.Driver).Include(dC => dC.Constructor).FirstOrDefaultAsync(dC => dC.Year == year &&
        dC.DriverId == driverId &&
        dC.ConstructorId == constructorId
        );


        if (driverConstructor == null)
        {
            return Error.NotFound("DriverConstructor not found");
        }
        return driverConstructor;
    }

    public async Task<ErrorOr<PagedResult<DriverConstructor>>> GetAllDriversConstructorsAsync(int pageNumber, int pageSize)
    {
        try
        {
            var pageSizeWithMaxLimit50 = pageSize;
            if (pageSizeWithMaxLimit50 > 50)
            {
                pageSizeWithMaxLimit50 = 50;
            }

            var totalCount = await _dataContext.DriverConstructors.CountAsync();

            var driversConstructors = await _dataContext.DriverConstructors.Include(dC => dC.Driver).Include(dC => dC.Constructor).Skip((pageNumber - 1) * pageSizeWithMaxLimit50)
            .Take(pageSizeWithMaxLimit50).ToListAsync();

            if (driversConstructors == null || driversConstructors.Count == 0)
            {
                return Error.NotFound("DriversConstructors not found");
            }

            var pagedResult = new PagedResult<DriverConstructor>(driversConstructors, totalCount);

            return pagedResult;
        }
        catch (Exception ex)
        {
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task<ErrorOr<PagedResult<DriverConstructor>>> GetAllDriversConstructorsByParamsAsync(GetAllDriversConstructorsByParamsRequest queryParams)
    {
        try
        {
            var query = _dataContext.DriverConstructors
            .Include(dc => dc.Driver)
            .Include(dc => dc.Constructor)
            .AsQueryable();

            if (queryParams.Year.HasValue)
            {
                query = query.Where(d => d.Year == queryParams.Year);
            }

            if (queryParams.DriverId.HasValue)
            {
                query = query.Where(d => d.DriverId == queryParams.DriverId);
            }

            if (queryParams.ConstructorId.HasValue)
            {
                query = query.Where(d => d.ConstructorId == queryParams.ConstructorId);
            }

            var pageSizeWithMaxLimit50 = queryParams.Pagesize ?? 15;
            if (pageSizeWithMaxLimit50 > 50)
            {
                pageSizeWithMaxLimit50 = 50;
            }

            int totalCount = await query.CountAsync();

            int skip = ((queryParams.Page ?? 1) - 1) * pageSizeWithMaxLimit50;
            query = query.Skip(skip).Take(pageSizeWithMaxLimit50);


            var driversconstructors = await query.ToListAsync();

            var pagedResult = new PagedResult<DriverConstructor>(driversconstructors, totalCount);

            return pagedResult;
        }
        catch (Exception ex)
        {
            return Error.Failure(description: ex.Message);
        }

    }

    public async Task<ErrorOr<DriverConstructor>> CreateDriverConstructorAsync(DriverConstructor driverConstructor)
    {
        try
        {
            _dataContext.DriverConstructors.Add(driverConstructor);
            await _dataContext.SaveChangesAsync();
            return driverConstructor;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ErrorOr<DriverConstructor>> UpdateDriverConstructorAsync(UpdateDriverConstructorRequest queryParams, UpdateDriverConstructorRequest bodyParams)
    {
        try
        {
            var query = _dataContext.DriverConstructors.AsQueryable();

            if (queryParams.Year.HasValue)
            {
                query = query.Where(d => d.Year == queryParams.Year);
            }

            if (queryParams.DriverId.HasValue)
            {
                query = query.Where(d => d.DriverId == queryParams.DriverId);
            }

            if (queryParams.ConstructorId.HasValue)
            {
                query = query.Where(d => d.ConstructorId == queryParams.ConstructorId);
            }


            var existingDriverConstructor = await query.FirstOrDefaultAsync();

            if (existingDriverConstructor == null)
            {
                return Error.NotFound("DriverConstructor not found");
            }

            _dataContext.DriverConstructors.Remove(existingDriverConstructor);

            var driverId = bodyParams.DriverId ?? queryParams.DriverId;
            var constructorId = bodyParams.ConstructorId ?? queryParams.ConstructorId;

            var existingDriver = await _dataContext.Drivers.FirstOrDefaultAsync(d => d.DriverId == driverId);

            var existingConstructor = await _dataContext.Constructors.FirstOrDefaultAsync(d => d.ConstructorId == constructorId);

            if (existingConstructor == null || existingDriver == null)
            {
                return Error.NotFound("Constructor or Driver not found");
            }

            var newDriverConstructor = new DriverConstructor(
                 bodyParams.Year ?? existingDriverConstructor.Year,
                 bodyParams.ConstructorId ?? existingDriverConstructor.ConstructorId,
                 bodyParams.ConstructorId ?? existingDriverConstructor.ConstructorId,
                 existingConstructor,
                 existingDriver
            );

            _dataContext.DriverConstructors.Add(newDriverConstructor);

            await _dataContext.SaveChangesAsync();

            return existingDriverConstructor;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ErrorOr<Deleted>> DeleteDriverConstructorAsync(DeleteDriverConstructorRequest queryParams)
    {
        try
        {
            var driverConstructor = await _dataContext.DriverConstructors.FirstOrDefaultAsync(dC => dC.ConstructorId == queryParams.ConstructorId && dC.DriverId == queryParams.DriverId && dC.Year == queryParams.Year);
            if (driverConstructor == null)
            {
                return Error.NotFound("DriverConstructor not found");
            }
            _dataContext.DriverConstructors.Remove(driverConstructor);
            await _dataContext.SaveChangesAsync();
            return new Deleted();
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }
    }
}
