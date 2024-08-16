using Microsoft.EntityFrameworkCore;
using F1RestAPI.Services.Drivers;
using F1RestAPI.Data;
using F1RestAPI.Models;
using F1RestAPI.Contracts.Drivers;
using Xunit.Abstractions;
using F1RestAPI.Services.Constructors;
using F1RestAPI.Contracts.Constructors;
using F1RestAPI.Contracts.DriversConstructors;
using F1RestAPI.Services.DriversConstructors;


namespace F1RestAPI.Tests.Services
{
    public class ServicesTests
    {
        private readonly ITestOutputHelper _output;

        public ServicesTests(ITestOutputHelper output)
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
        public async Task DriverService_ShouldAddRetrieveUpdateAndDeleteDrivers()
        {
            try
            {
                var uniqueDbName = Guid.NewGuid().ToString();
                var context = GetInMemoryDbContext(uniqueDbName);
                var driverService = new DriverService(context);

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

                var maxVerstappen = new Driver(
                    driverId: 33,
                    driverRef: "maxverstappen",
                    driverNumber: 33,
                    driverCode: "MAX",
                    driverForename: "Max",
                    driverSurname: "Verstappen",
                    dateOfBirth: new DateTime(1997, 9, 30),
                    nationality: "Dutch",
                    wikipediaUrl: "https://en.wikipedia.org/wiki/Max_Verstappen",
                    polePositions: 18,
                    wins: 45,
                    podiumns: 100,
                    dnf: 15
                );

                var josVerstappen = new Driver(
                    driverId: 19,
                    driverRef: "josverstappen",
                    driverNumber: 19,
                    driverCode: "JOS",
                    driverForename: "Jos",
                    driverSurname: "Verstappen",
                    dateOfBirth: new DateTime(1972, 3, 4),
                    nationality: "Dutch",
                    wikipediaUrl: "https://en.wikipedia.org/wiki/Jos_Verstrappen",
                    polePositions: 3,
                    wins: 0,
                    podiumns: 6,
                    dnf: 14
                );


                var result = await driverService.CreateDriverAsync(fakeDriver);
                await driverService.CreateDriverAsync(maxVerstappen);
                await driverService.CreateDriverAsync(josVerstappen);


                Assert.False(result.IsError);
                Assert.Equal(result.Value.DriverForename, fakeDriver.DriverForename);
                Assert.Equal(result.Value.DriverSurname, fakeDriver.DriverSurname);

                var updatedRequest = new UpdateDriverRequest(
                    DriverRef: "SENNA01",
                    DriverNumber: 1,
                    DriverCode: "SEN",
                    DriverForename: "Ayrton",
                    DriverSurname: "Senna",
                    DateOfBirth: new DateTime(1960, 3, 21),
                    Nationality: "Brazilian",
                    WikipediaUrl: "https://en.wikipedia.org/wiki/Ayrton_Senna",
                    PolePositions: 65,
                    Wins: null,
                    Podiumns: null,
                    DNF: 14
                );

                await driverService.UpdateDriverAsync(1, updatedRequest);

                var updatedResult = await driverService.GetDriverById(1);

                Assert.False(updatedResult.IsError);
                Assert.Equal(15, updatedResult.Value.Podiumns);
                Assert.NotEqual("john doe", updatedRequest.DriverRef);
                Assert.NotEqual("American", updatedRequest.Nationality);

                var getAllDrivers = await driverService.GetAllDriversAsync(1, 15);

                Assert.False(getAllDrivers.IsError);
                Assert.Equal(3, getAllDrivers.Value.TotalCount);


                var requestParams = new GetAllDriversByParamsRequest(
                    Page: null,
                    Pagesize: null,
                    DriverRef: null,
                    DriverNumber: null,
                    DriverCode: null,
                    DriverForename: null,
                    DriverSurname: "Verstappen",
                    DateOfBirth: null,
                    Nationality: null,
                    WikipediaUrl: null,
                    PolePositions: null,
                    Wins: null,
                    Podiumns: null,
                    DNF: null,
                    ConstructorIds: null
                );

                var getBySurname = await driverService.GetAllDriversByParamsAsync(requestParams);

                Assert.False(getBySurname.IsError);
                Assert.Equal(2, getBySurname.Value.TotalCount);

                foreach (var driver in getBySurname.Value.Items)
                {
                    Assert.Equal("Verstappen", driver.DriverSurname);
                }

                await driverService.DeleteDriverAsync(1);

                var newGetAllDrivers = await driverService.GetAllDriversAsync(1, 15);

                Assert.False(newGetAllDrivers.IsError);
                Assert.Equal(2, newGetAllDrivers.Value.TotalCount);

                _output.WriteLine("PASSED: DriverService_ShouldAddRetrieveUpdateAndDeleteDrivers");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"FAILED: DriverService_ShouldAddRetrieveUpdateAndDeleteDrivers -- {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task ConstructorService_ShouldAddRetrieveUpdateAndDeleteConstructors()
        {
            try
            {
                var uniqueDbName = Guid.NewGuid().ToString();
                var context = GetInMemoryDbContext(uniqueDbName);
                var constructorService = new ConstructorService(context);

                var fakeConstructor = new Constructor(
                    1,
                    "mclaren",
                    "McLaren",
                    "British",
                    "https://en.wikipedia.org/wiki/Team_Name"
                );
                var fakeConstructor2 = new Constructor(
                    2,
                    "mercedes",
                    "Mercedes",
                    "British",
                    "https://en.wikipedia.org/wiki/Team_Name"
                );

                var result = await constructorService.CreateConstructorAsync(fakeConstructor);
                await constructorService.CreateConstructorAsync(fakeConstructor2);

                Assert.False(result.IsError);
                Assert.Equal(result.Value.Nationality, fakeConstructor.Nationality);
                Assert.Equal(result.Value.Name, fakeConstructor.Name);

                var updateConstructor = new UpdateConstructorRequest(
                    ConstructorRef: "ferrari",
                    Name: "Ferrari",
                    Nationality: "Italian",
                    Url: null
                );

                await constructorService.UpdateConstructorAsync(1, updateConstructor);

                var updatedResult = await constructorService.GetConstructorById(1);

                Assert.False(updatedResult.IsError);
                Assert.Equal("ferrari", updatedResult.Value.ConstructorRef);
                Assert.NotEqual("British", updatedResult.Value.Nationality);
                Assert.NotEqual("McLaren", updatedResult.Value.Name);

                var getAllConstructors = await constructorService.GetAllConstructorsAsync(1, 15);

                Assert.False(getAllConstructors.IsError);
                Assert.Equal(2, getAllConstructors.Value.TotalCount);


                var requestParams = new GetAllConstructorsByParamsRequest(
                    Page: 1,
                    Pagesize: 15,
                    ConstructorRef: "ferrari",
                    Name: "Ferrari",
                    Nationality: "Italian",
                    Url: null,
                    DriversIds: null
                );

                var getConstructorByParamsResult = await constructorService.GetAllConstructorsByParamsAsync(requestParams);

                Assert.False(getConstructorByParamsResult.IsError);
                Assert.Equal(1, getConstructorByParamsResult.Value.TotalCount);

                await constructorService.DeleteConstructorAsync(1);

                var getDeleteConstructor = await constructorService.GetConstructorById(1);

                Assert.True(getDeleteConstructor.IsError);

                _output.WriteLine("PASSED: ConstructorService_ShouldAddRetrieveUpdateAndDeleteConstructors");
            }
            catch (Exception ex)
            {
                _output.WriteLine($"FAILED: ConstructorService_ShouldAddRetrieveUpdateAndDeleteConstructors -- {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task DriverConstructorService_ShouldAddRetrieveUpdateAndDeleteDriversConstructors()
        {
            try
            {
                var uniqueDbName = Guid.NewGuid().ToString();
                var context = GetInMemoryDbContext(uniqueDbName);
                var constructorService = new ConstructorService(context);
                var driversService = new DriverService(context);
                var driverConstructorService = new DriverConstructorService(context);

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
                    "mclaren",
                    "McLaren",
                    "British",
                    "https://en.wikipedia.org/wiki/Team_Name"
                );

                await driversService.CreateDriverAsync(fakeDriver);
                await constructorService.CreateConstructorAsync(fakeConstructor);

                var fakeDriverConstructor = new DriverConstructor(
                    constructorId: 1,
                    driverId: 1,
                    year: 2000,
                    driver: fakeDriver,
                    constructor: fakeConstructor
                );

                await driverConstructorService.CreateDriverConstructorAsync(fakeDriverConstructor);
                var result = await driverConstructorService.GetDriverConstructorByIdAsync(2000, 1, 1);

                Assert.False(result.IsError);
                Assert.Equal(2000, result.Value.Year);
                Assert.Equal(1, result.Value.DriverId);
                Assert.Equal(1, result.Value.ConstructorId);

                var driverConstructorToUpdateRequest = new UpdateDriverConstructorRequest(
                    Year: 2000,
                    ConstructorId: 1,
                    DriverId: 1
                );
                var driverConstructorDataToUpdateRequest = new UpdateDriverConstructorRequest(
                    Year: 2010,
                    ConstructorId: null,
                    DriverId: null
                );

                await driverConstructorService.UpdateDriverConstructorAsync(driverConstructorToUpdateRequest, driverConstructorDataToUpdateRequest);

                var result2 = await driverConstructorService.GetDriverConstructorByIdAsync(2010, 1, 1);

                Assert.False(result2.IsError);
                Assert.Equal(2010, result2.Value.Year);

                var driverConstructorToDeleteRequest = new DeleteDriverConstructorRequest(
                    Year: 2010,
                    ConstructorId: 1,
                    DriverId: 1
                );

                await driverConstructorService.DeleteDriverConstructorAsync(driverConstructorToDeleteRequest);

                var result3 = await driverConstructorService.GetDriverConstructorByIdAsync(2010, 1, 1);

                Assert.True(result3.IsError);

                _output.WriteLine("PASSED: DriverConstructorService_ShouldAddRetrieveUpdateAndDeleteDriversConstructors");

            }
            catch (Exception ex)
            {
                _output.WriteLine($"FAILED: DriverConstructorService_ShouldAddRetrieveUpdateAndDeleteDriversConstructors -- {ex.Message}");
                throw;
            }
        }

        [Fact]
        public async Task DriverConstructorService_ShouldCascadeOnDelete()
        {
            try
            {
                var uniqueDbName = Guid.NewGuid().ToString();
                var context = GetInMemoryDbContext(uniqueDbName);
                var constructorService = new ConstructorService(context);
                var driversService = new DriverService(context);
                var driverConstructorService = new DriverConstructorService(context);

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

                var fakeDriver2 = new Driver(
                    driverId: 2,
                    driverRef: "maxverstappen",
                    driverNumber: 33,
                    driverCode: "MAX",
                    driverForename: "Max",
                    driverSurname: "Verstappen",
                    dateOfBirth: new DateTime(1997, 9, 30),
                    nationality: "Dutch",
                    wikipediaUrl: "https://en.wikipedia.org/wiki/Max_Verstappen",
                    polePositions: 18,
                    wins: 45,
                    podiumns: 100,
                    dnf: 15
                );

                var fakeConstructor = new Constructor(
                    1,
                    "mclaren",
                    "McLaren",
                    "British",
                    "https://en.wikipedia.org/wiki/Team_Name"
                );

                await driversService.CreateDriverAsync(fakeDriver);
                await driversService.CreateDriverAsync(fakeDriver2);
                await constructorService.CreateConstructorAsync(fakeConstructor);

                var fakeDriverConstructor = new DriverConstructor(
                    constructorId: 1,
                    driverId: 1,
                    year: 2000,
                    driver: fakeDriver,
                    constructor: fakeConstructor
                );
                var fakeDriverConstructor2 = new DriverConstructor(
                    constructorId: 1,
                    driverId: 2,
                    year: 2000,
                    driver: fakeDriver,
                    constructor: fakeConstructor
                );

                await driverConstructorService.CreateDriverConstructorAsync(fakeDriverConstructor);
                await driverConstructorService.CreateDriverConstructorAsync(fakeDriverConstructor2);

                await driversService.DeleteDriverAsync(1);

                var result = await driverConstructorService.GetDriverConstructorByIdAsync(2000, 1, 1);

                Assert.True(result.IsError);

                await constructorService.DeleteConstructorAsync(1);

                var result2 = await driverConstructorService.GetDriverConstructorByIdAsync(2000, 2, 1);

                Assert.True(result2.IsError);

            }
            catch (Exception ex)
            {
                _output.WriteLine($"FAILED: DriverConstructorService -- {ex.Message}");
                throw;
            }
        }
    }
}