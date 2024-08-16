namespace F1RestAPI.Models;

public class IpCountryState
{
    public string Country { get; set; }
    public string City { get; set; }
    public int Count { get; set; }

    public IpCountryState() { }

    public IpCountryState(string country, string city, int count)
    {
        Country = country;
        City = city;
        Count = count;
    }
}