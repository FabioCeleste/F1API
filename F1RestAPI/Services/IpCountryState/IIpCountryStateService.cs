using ErrorOr;
using F1RestAPI.Contracts.IpAPI;
using F1RestAPI.Models;

namespace F1RestAPI.Services.IpCountryStateServices;

public interface IIpCountryStateService
{
    Task<ErrorOr<IpAPIResponse>> GetIpInfo(string ipAddress);
    Task SaveConnectionCountToCountryState(IEnumerable<IpConnectionCount> ipConnectionsCounts);
    Task RecordConnection();

}