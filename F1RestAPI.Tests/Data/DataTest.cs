using Microsoft.EntityFrameworkCore;
using F1RestAPI.Data;
using F1RestAPI.Models;
using Xunit.Abstractions;

namespace F1RestAPI.Tests.Data
{
    public class DataContextTests
    {
        private readonly ITestOutputHelper _output;

        public DataContextTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private DataContext GetInMemoryDbContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            return new DataContext(options);
        }

        [Fact]
        public async Task CanAddRetrieveUpdateAndDeleteDriver()
        {
            try
            {
                var uniqueDbName = Guid.NewGuid().ToString();
                var context = GetInMemoryDbContext(uniqueDbName);
                var fakeDriver = new Driver(
                    driverId: 1,
                    driverRef: "john doe",
                    driverNumber: 99,
                    driverCode: "FD99",
                    driverForename: "John",
                    driverSurname: "Doe",
                    dateOfBirth: new DateTime(1985, 6, 15),
                    nationality: "American",
                    wikipediaUrl: "https://en.wikipedia.org/wiki/John_Doe",
                    polePositions: 10,
                    wins: 5,
                    podiumns: 15,
                    dnf: 2
                );

                var anotherDriver = new Driver(
                    driverId: 2,
                    driverRef: "jane smith",
                    driverNumber: 88,
                    driverCode: "JS88",
                    driverForename: "Jane",
                    driverSurname: "Smith",
                    dateOfBirth: new DateTime(1990, 4, 22),
                    nationality: "British",
                    wikipediaUrl: "https://en.wikipedia.org/wiki/Jane_Smith",
                    polePositions: 8,
                    wins: 6,
                    podiumns: 12,
                    dnf: 3
                );

                context.Drivers.Add(fakeDriver);
                context.Drivers.Add(anotherDriver);
                await context.SaveChangesAsync();
                var retrievedDriver = await context.Drivers.FirstOrDefaultAsync(d => d.DriverId == 1);
                var retrievedDriver2 = await context.Drivers.FirstOrDefaultAsync(d => d.DriverId == 2);

                Assert.NotNull(retrievedDriver);
                Assert.Equal("John", retrievedDriver.DriverForename);
                Assert.Equal("Doe", retrievedDriver.DriverSurname);

                Assert.NotNull(retrievedDriver2);
                Assert.Equal("Jane", retrievedDriver2.DriverForename);
                Assert.Equal("Smith", retrievedDriver2.DriverSurname);

                anotherDriver.DriverForename = "new";
                anotherDriver.DriverSurname = "name";

                await context.SaveChangesAsync();

                var retrievedDriver3 = await context.Drivers.FirstOrDefaultAsync(d => d.DriverId == 2);

                Assert.NotNull(retrievedDriver3);
                Assert.NotEqual("Jane", retrievedDriver3.DriverForename);
                Assert.NotEqual("Smith", retrievedDriver3.DriverSurname);

                context.Remove(retrievedDriver);

                await context.SaveChangesAsync();

                var retrievedDriver4 = await context.Drivers.FirstOrDefaultAsync(d => d.DriverId == 1);

                Assert.Null(retrievedDriver4);

                _output.WriteLine("PASSED: CanAddRetrieveUpdateAndDeleteDriver");

            }
            catch (Exception ex)
            {
                _output.WriteLine($"FAILED: CanAddRetrieveUpdateAndDeleteDriver -- {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task CanAddRetrieveUpdateAndDeleteConstructor()
        {
            try
            {
                var uniqueDbName = Guid.NewGuid().ToString();
                var context = GetInMemoryDbContext(uniqueDbName);
                var fakeConstructor = new Constructor(
                    1,
                    "teamRef",
                    "Team Name",
                    "British",
                    "https://en.wikipedia.org/wiki/Team_Name"
                );

                var anotherConstructor = new Constructor(
                    2,
                    "teamRef2",
                    "Second Team",
                    "German",
                    "https://en.wikipedia.org/wiki/Second_Team"
                );

                context.Constructors.Add(fakeConstructor);
                context.Constructors.Add(anotherConstructor);
                await context.SaveChangesAsync();
                var retrievedConstructor = await context.Constructors.FirstOrDefaultAsync(d => d.ConstructorId == 1);
                var retrievedConstructor2 = await context.Constructors.FirstOrDefaultAsync(d => d.ConstructorId == 2);

                Assert.NotNull(retrievedConstructor);
                Assert.Equal("British", retrievedConstructor.Nationality);
                Assert.Equal("Team Name", retrievedConstructor.Name);

                Assert.NotNull(retrievedConstructor2);
                Assert.Equal("German", retrievedConstructor2.Nationality);
                Assert.Equal("Second Team", retrievedConstructor2.Name);

                anotherConstructor.Nationality = "Brazilian";
                anotherConstructor.Name = "Copersucar";

                await context.SaveChangesAsync();

                var retrievedConstructor3 = await context.Constructors.FirstOrDefaultAsync(d => d.ConstructorId == 2);

                Assert.NotNull(retrievedConstructor3);
                Assert.NotEqual("Jane", retrievedConstructor3.Nationality);
                Assert.NotEqual("Smith", retrievedConstructor3.Name);

                context.Remove(retrievedConstructor);

                await context.SaveChangesAsync();

                var retrievedConstructor4 = await context.Drivers.FirstOrDefaultAsync(d => d.DriverId == 1);

                Assert.Null(retrievedConstructor4);

                _output.WriteLine("PASSED: CanAddRetrieveUpdateAndDeleteConstructor");

            }
            catch (Exception ex)
            {
                _output.WriteLine($"FAILED: CanAddRetrieveUpdateAndDeleteConstructor -- {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task CanAddRetrieveUpdateAndDeleteDriverConstructor()
        {
            try
            {
                var uniqueDbName = Guid.NewGuid().ToString();
                var context = GetInMemoryDbContext(uniqueDbName);

                var fakeDriver = new Driver(
                    driverId: 1,
                    driverRef: "john doe",
                    driverNumber: 99,
                    driverCode: "FD99",
                    driverForename: "John",
                    driverSurname: "Doe",
                    dateOfBirth: new DateTime(1985, 6, 15),
                    nationality: "American",
                    wikipediaUrl: "https://en.wikipedia.org/wiki/John_Doe",
                    polePositions: 10,
                    wins: 5,
                    podiumns: 15,
                    dnf: 2
                );

                var fakeConstructor = new Constructor(
                    1,
                    "teamRef",
                    "Team Name",
                    "British",
                    "https://en.wikipedia.org/wiki/Team_Name"
                );

                context.Drivers.Add(fakeDriver);
                context.Constructors.Add(fakeConstructor);

                await context.SaveChangesAsync();

                var fakeDriverConstructor = new DriverConstructor(
                    2000,
                    1,
                    1,
                    fakeConstructor,
                    fakeDriver
                );

                context.DriverConstructors.Add(fakeDriverConstructor);

                await context.SaveChangesAsync();

                var retrievedDriverConstructor = await context.DriverConstructors.FirstOrDefaultAsync(dC => dC.Year == 2000 && dC.ConstructorId == 1 && dC.DriverId == 1);

                Assert.NotNull(retrievedDriverConstructor);

                context.Remove(retrievedDriverConstructor);

                await context.SaveChangesAsync();

                var deletedDriverConstructor = await context.DriverConstructors.FirstOrDefaultAsync(dC => dC.Year == 2000 && dC.ConstructorId == 1 && dC.DriverId == 1);

                Assert.Null(deletedDriverConstructor);
            }
            catch (Exception ex)
            {
                _output.WriteLine($"FAILED: CanAddRetrieveUpdateAndDeleteDriver -- {ex.Message}");
                throw;
            }
        }
    }
}
