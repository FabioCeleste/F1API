using F1RestAPI.Models;
using ErrorOr;
using F1RestAPI.Data;
using Microsoft.EntityFrameworkCore;
using F1RestAPI.Contracts.Pagination;
using F1RestAPI.Contracts.Drivers;

namespace F1RestAPI.Services.Drivers;

public class DriverService : IDriverService
{
    private readonly DataContext _dataContext;

    public DriverService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<ErrorOr<Driver>> GetDriverById(int id)
    {
        try
        {
            var driver = await _dataContext.Drivers.Include(driver => driver.DriverConstructors).ThenInclude(dC => dC.Constructor).FirstOrDefaultAsync(d => d.DriverId == id);

            if (driver == null)
            {
                return Error.NotFound("Driver not found");
            }

            driver.DriverConstructors = driver.DriverConstructors
                .OrderBy(dC => dC.Year)
                .ToList();

            return driver;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }

    }
    public async Task<ErrorOr<PagedResult<Driver>>> GetAllDriversAsync(int pageNumber, int pageSize)
    {
        try
        {
            var pageSizeWithMaxLimit50 = pageSize;
            if (pageSizeWithMaxLimit50 > 50)
            {
                pageSizeWithMaxLimit50 = 50;
            }

            var totalCount = await _dataContext.Drivers.CountAsync();

            var drivers = await _dataContext.Drivers.OrderBy(d => d.DriverId).Skip((pageNumber - 1) * pageSizeWithMaxLimit50)
            .Take(pageSizeWithMaxLimit50).ToListAsync();

            if (drivers == null || drivers.Count == 0)
            {
                return Error.NotFound("Drivers not found");
            }

            var pagedResult = new PagedResult<Driver>(drivers, totalCount);

            return pagedResult;

        }
        catch (Exception ex)
        {
            return Error.Failure(description: ex.Message);
        }

    }

    public async Task<ErrorOr<PagedResult<Driver>>> GetAllDriversByParamsAsync(GetAllDriversByParamsRequest queryParams)
    {
        try
        {
            var query = _dataContext.Drivers.AsQueryable();

            if (!string.IsNullOrEmpty(queryParams.DriverRef))
            {
                query = query.Where(d => d.DriverRef == queryParams.DriverRef);
            }

            if (queryParams.DriverNumber.HasValue)
            {
                query = query.Where(d => d.DriverNumber == queryParams.DriverNumber);
            }

            if (!string.IsNullOrEmpty(queryParams.DriverCode))
            {
                query = query.Where(d => d.DriverCode == queryParams.DriverCode);
            }

            if (!string.IsNullOrEmpty(queryParams.DriverForename))
            {
                query = query.Where(d => d.DriverForename == queryParams.DriverForename);
            }

            if (!string.IsNullOrEmpty(queryParams.DriverSurname))
            {
                query = query.Where(d => d.DriverSurname == queryParams.DriverSurname);
            }

            if (queryParams.DateOfBirth.HasValue)
            {
                query = query.Where(d => d.DateOfBirth == queryParams.DateOfBirth);
            }

            if (!string.IsNullOrEmpty(queryParams.Nationality))
            {
                query = query.Where(d => d.Nationality == queryParams.Nationality);
            }

            if (!string.IsNullOrEmpty(queryParams.WikipediaUrl))
            {
                query = query.Where(d => d.WikipediaUrl == queryParams.WikipediaUrl);
            }

            if (queryParams.PolePositions.HasValue)
            {
                query = query.Where(d => d.PolePositions == queryParams.PolePositions);
            }

            if (queryParams.Wins.HasValue)
            {
                query = query.Where(d => d.Wins == queryParams.Wins);
            }

            if (queryParams.Podiumns.HasValue)
            {
                query = query.Where(d => d.Podiumns == queryParams.Podiumns);
            }

            if (queryParams.DNF.HasValue)
            {
                query = query.Where(d => d.DNF == queryParams.DNF);
            }

            int totalCount = await query.CountAsync();

            var pageSizeWithMaxLimit50 = queryParams.Pagesize ?? 15;
            if (pageSizeWithMaxLimit50 > 50)
            {
                pageSizeWithMaxLimit50 = 50;
            }


            int skip = ((queryParams.Page ?? 1) - 1) * pageSizeWithMaxLimit50;
            query = query.Skip(skip).Take(pageSizeWithMaxLimit50);

            var drivers = await query.ToListAsync();

            var pagedResult = new PagedResult<Driver>(drivers, totalCount);

            return pagedResult;
        }
        catch (Exception ex)
        {
            return Error.Failure(description: ex.Message);
        }

    }

    public async Task<ErrorOr<Driver>> CreateDriverAsync(Driver driver)
    {
        try
        {
            _dataContext.Drivers.Add(driver);

            await _dataContext.SaveChangesAsync();

            return driver;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ErrorOr<Driver>> UpdateDriverAsync(int id, UpdateDriverRequest newData)
    {
        try
        {
            var existingDriver = await _dataContext.Drivers.FindAsync(id);

            if (existingDriver == null)
            {
                return Error.NotFound("Drivers not found");
            }

            existingDriver.DriverRef = newData.DriverRef ?? existingDriver.DriverRef;
            existingDriver.DriverNumber = newData.DriverNumber ?? existingDriver.DriverNumber;
            existingDriver.DriverCode = newData.DriverCode ?? existingDriver.DriverCode;
            existingDriver.DriverForename = newData.DriverForename ?? existingDriver.DriverForename;
            existingDriver.DriverSurname = newData.DriverSurname ?? existingDriver.DriverSurname;
            existingDriver.DateOfBirth = newData.DateOfBirth ?? existingDriver.DateOfBirth;
            existingDriver.Nationality = newData.Nationality ?? existingDriver.Nationality;
            existingDriver.WikipediaUrl = newData.WikipediaUrl ?? existingDriver.WikipediaUrl;
            existingDriver.PolePositions = newData.PolePositions ?? existingDriver.PolePositions;
            existingDriver.Wins = newData.Wins ?? existingDriver.Wins;
            existingDriver.Podiumns = newData.Podiumns ?? existingDriver.Podiumns;
            existingDriver.DNF = newData.DNF ?? existingDriver.DNF;

            await _dataContext.SaveChangesAsync();
            return existingDriver;
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }
    }

    public async Task<ErrorOr<Deleted>> DeleteDriverAsync(int id)
    {
        try
        {
            var driver = await _dataContext.Drivers.FindAsync(id);
            if (driver == null)
            {
                return Error.NotFound("Driver not found");
            }
            _dataContext.Drivers.Remove(driver);
            await _dataContext.SaveChangesAsync();
            return new Deleted();
        }
        catch (Exception ex)
        {
            return Error.Failure($"An error occurred: {ex.Message}");
        }
    }
}
