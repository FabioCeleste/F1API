
namespace F1RestAPI.Models
{
    public class Driver
    {
        public int DriverId { get; set; }
        public string DriverRef { get; set; }
        public int DriverNumber { get; set; }
        public string DriverCode { get; set; }
        public string DriverForename { get; set; }
        public string DriverSurname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string WikipediaUrl { get; set; }
        public int PolePositions { get; set; }
        public int Wins { get; set; }
        public int Podiumns { get; set; }
        public int DNF { get; set; }
        public IEnumerable<DriverConstructor> DriverConstructors { get; set; }

        public Driver() { }

        public Driver(
           int driverId,
           string driverRef,
           int driverNumber,
           string driverCode,
           string driverForename,
           string driverSurname,
           DateTime dateOfBirth,
           string nationality,
           string wikipediaUrl,
           int polePositions,
           int wins,
           int podiumns,
           int dnf,
           IEnumerable<DriverConstructor>? driverConstructors = null)
        {
            DriverId = driverId;
            DriverRef = driverRef;
            DriverNumber = driverNumber;
            DriverCode = driverCode;
            DriverForename = driverForename;
            DriverSurname = driverSurname;
            DateOfBirth = dateOfBirth;
            Nationality = nationality;
            WikipediaUrl = wikipediaUrl;
            PolePositions = polePositions;
            Wins = wins;
            Podiumns = podiumns;
            DNF = dnf;
            DriverConstructors = driverConstructors ?? new List<DriverConstructor>();
        }
    }
}
