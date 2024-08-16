namespace F1RestAPI.Models;

public class IpConnectionCount
{
    public string Ip { get; set; }
    public int Count { get; set; }

    public IpConnectionCount() { }

    public IpConnectionCount(string ip, int count)
    {
        Ip = ip;
        Count = count;
    }
}