using ErrorOr;
using F1RestAPI.Models;
namespace F1RestAPI.Services.IpConnectionCounts;

public interface IIpConnectionCount
{
    Task<ErrorOr<List<IpConnectionCount>>> Get15Connections();
    Task AddNewCountToConnections(string ip);
}