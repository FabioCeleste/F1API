namespace F1RestAPI.Models
{
    public class Constructor
    {
        public int ConstructorId { get; set; }
        public string ConstructorRef { get; set; }
        public string Name { get; set; }
        public string Nationality { get; set; }
        public string Url { get; set; }
        public IEnumerable<DriverConstructor> DriverConstructors { get; set; }
        public Constructor() { }
        public Constructor(
            int constructorId,
            string constructorRef,
            string name,
            string nationality,
            string url,
            IEnumerable<DriverConstructor>? driverConstructors = null)
        {
            ConstructorId = constructorId;
            ConstructorRef = constructorRef;
            Name = name;
            Nationality = nationality;
            Url = url;
            DriverConstructors = driverConstructors ?? new List<DriverConstructor>();
        }
    }
}