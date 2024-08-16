
using F1RestAPI.Contracts.IpAPI;
using Newtonsoft.Json;
using ErrorOr;
using F1RestAPI.Data;
using F1RestAPI.Models;
using Microsoft.EntityFrameworkCore;
using Prometheus;

namespace F1RestAPI.Services.IpCountryStateServices;
public class IpCountryStateService : IIpCountryStateService
{
    private readonly HttpClient _httpClient;
    private readonly DataContext _dataContext;
    private static readonly Gauge CountryCityConnectionGauge = Metrics
        .CreateGauge("country_state_connection_count", "Current number of connections per country and city",
            new GaugeConfiguration
            {
                LabelNames = new[] { "country", "city" }
            });

    public IpCountryStateService(HttpClient httpClient, DataContext dataContext)
    {
        _httpClient = httpClient;
        _dataContext = dataContext;
    }

    public async Task<ErrorOr<IpAPIResponse>> GetIpInfo(string ipAddress)
    {
        try
        {
            var requestUri = $"http://ip-api.com/json/{ipAddress}";

            var response = await _httpClient.GetStringAsync(requestUri);

            if (!string.IsNullOrEmpty(response))
            {
                var geolocationResponse = JsonConvert.DeserializeObject<IpAPIResponse>(response);

                if (geolocationResponse != null)
                {
                    return geolocationResponse;
                }
                else
                {
                    return Error.Failure(description: "Failed to deserialize the response.");
                }
            }
            else
            {
                return Error.Failure(description: "Empty response from the API.");
            }
        }
        catch (Exception ex)
        {
            return Error.Failure(description: ex.Message);
        }
    }

    public async Task SaveConnectionCountToCountryState(IEnumerable<IpConnectionCount> ipConnectionsCounts)
    {
        try
        {
            foreach (var ipConnection in ipConnectionsCounts)
            {
                var ipInfo = await GetIpInfo(ipConnection.Ip);
                if (ipInfo.IsError)
                {
                    return;
                }

                try
                {
                    var rowsAffected = await _dataContext.Database.ExecuteSqlRawAsync(
                    @"UPDATE ""IpCountryStates""
                    SET ""Count"" = ""Count"" + {2}
                    WHERE ""Country"" = {0} AND ""City"" = {1};",
                    ipInfo.Value.country,
                    ipInfo.Value.city,
                    ipConnection.Count
                );

                    if (rowsAffected == 0)
                    {
                        await _dataContext.Database.ExecuteSqlRawAsync(
                            @"INSERT INTO ""IpCountryStates"" (""Country"", ""City"", ""Count"")
                        VALUES ({0}, {1}, {2});",
                            ipInfo.Value.country,
                            ipInfo.Value.city,
                            ipConnection.Count
                        );
                    }

                }
                catch (System.Exception)
                {
                    Console.WriteLine("Skiping IP");
                }

                _dataContext.IpConnectionCounts.Remove(ipConnection);
                await _dataContext.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    public async Task RecordConnection()
    {
        var ipconnections = await _dataContext.IpCountryStates.ToListAsync();

        if (ipconnections == null || ipconnections.Count == 0)
        {
            return;
        }

        foreach (var c in ipconnections)
        {
            CountryCityConnectionGauge
                      .WithLabels(c.Country, c.City)
                      .Set(c.Count);
        }
    }
}
