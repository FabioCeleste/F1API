namespace F1RestAPI.Models
{
    public class DriverConstructor
    {
        public int Year { get; set; }
        public int ConstructorId { get; set; }
        public int DriverId { get; set; }

        public Driver Driver { get; }
        public Constructor Constructor { get; }

        public DriverConstructor() { }

        public DriverConstructor(
           int year,
           int constructorId,
           int driverId,
           Constructor constructor,
           Driver driver
   )
        {
            Year = year;
            ConstructorId = constructorId;
            DriverId = driverId;
            Constructor = constructor;
            Driver = driver;
        }
    }
}
