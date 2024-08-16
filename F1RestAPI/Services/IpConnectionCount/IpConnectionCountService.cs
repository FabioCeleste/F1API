using ErrorOr;
using F1RestAPI.Data;
using F1RestAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace F1RestAPI.Services.IpConnectionCounts;

public class IpConnectionCountService : IIpConnectionCount
{
    private readonly HttpClient _httpClient;
    private readonly DataContext _dataContext;

    public IpConnectionCountService(HttpClient httpClient, DataContext dataContext)
    {
        _httpClient = httpClient;
        _dataContext = dataContext;
    }

    public async Task<ErrorOr<List<IpConnectionCount>>> Get15Connections()
    {
        try
        {
            var connections = await _dataContext.IpConnectionCounts.Take(15).ToListAsync();

            if (connections == null)
            {
                return Error.NotFound("Drivers not found");
            }

            return connections;
        }
        catch (Exception ex)
        {
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task AddNewCountToConnections(string ip)
    {
        try
        {
            var rowsAffected = await _dataContext.Database.ExecuteSqlRawAsync(
                @"UPDATE ""IpConnectionCounts""
                SET ""Count"" = ""Count"" + 1
                WHERE ""Ip"" = {0};",
                ip
            );

            if (rowsAffected == 0)
            {
                await _dataContext.Database.ExecuteSqlRawAsync(
                    @"INSERT INTO ""IpConnectionCounts"" (""Ip"", ""Count"")
                     VALUES ({0}, 1);",
                    ip
                );
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

}