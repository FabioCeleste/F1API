using Microsoft.AspNetCore.Mvc;
using F1RestAPI.Contracts.DriversConstructors;
using F1RestAPI.Services.DriversConstructors;
using F1RestAPI.Services.Drivers;
using F1RestAPI.Services.Constructors;
using F1RestAPI.Models;
using F1RestAPI.Contracts.Drivers;
using F1RestAPI.Contracts.Constructors;
using F1RestAPI.Contracts.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace DriversApi.Controllers
{
    [ApiController]
    [Route("api/driversconstructors")]
    public class DriversConstructorsController : ControllerBase
    {
        private readonly IDriverConstructorService _driverConstructorService;
        private readonly IConstructorService _constructorService;
        private readonly IDriverService _driverService;
        public DriversConstructorsController(IDriverConstructorService driverConstructorService,
            IConstructorService constructorService,
            IDriverService driverService)
        {
            _driverConstructorService = driverConstructorService;
            _constructorService = constructorService;
            _driverService = driverService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<PagedResult<GetAllDriversConstructorsResponse>>> GetAllDriversConstructors([FromQuery] GetAllDriversConstructorsRequest query)
        {
            try
            {
                var result = await _driverConstructorService.GetAllDriversConstructorsAsync(query.Page ?? 1, query.Pagesize ?? 25);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });

                }

                var driversConstructors = result.Value.Items.Select(dC => new GetAllDriversConstructorsResponse(
                    dC.Year,
                    dC.ConstructorId,
                    dC.DriverId,
                    new DriverDto(
                        dC.Driver.DriverId,
                        dC.Driver.DriverRef,
                        dC.Driver.DriverNumber,
                        dC.Driver.DriverCode,
                        dC.Driver.DriverForename,
                        dC.Driver.DriverSurname,
                        dC.Driver.DateOfBirth,
                        dC.Driver.Nationality,
                        dC.Driver.WikipediaUrl,
                        dC.Driver.PolePositions,
                        dC.Driver.Wins,
                        dC.Driver.Podiumns,
                        dC.Driver.DNF),
                         new ConstructorDto(
                            dC.Constructor.ConstructorId,
                            dC.Constructor.ConstructorRef,
                            dC.Constructor.Name,
                            dC.Constructor.Nationality,
                            dC.Constructor.Url)
                    )).ToList();

                var pagedResult = new PagedResult<GetAllDriversConstructorsResponse>(driversConstructors, result.Value.TotalCount);

                return Ok(pagedResult);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }


        }

        [HttpGet("byparams")]
        public async Task<ActionResult<PagedResult<GetAllDriversConstructorsResponse>>> GetAllDriversConstructorsByParams([FromQuery] GetAllDriversConstructorsByParamsRequest query)
        {
            try
            {
                var result = await _driverConstructorService.GetAllDriversConstructorsByParamsAsync(query);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });

                }

                var driversConstructors = result.Value.Items.Select(dC => new GetAllDriversConstructorsResponse(
                    dC.Year,
                    dC.ConstructorId,
                    dC.DriverId,
                    new DriverDto(
                        dC.Driver.DriverId,
                        dC.Driver.DriverRef,
                        dC.Driver.DriverNumber,
                        dC.Driver.DriverCode,
                        dC.Driver.DriverForename,
                        dC.Driver.DriverSurname,
                        dC.Driver.DateOfBirth,
                        dC.Driver.Nationality,
                        dC.Driver.WikipediaUrl,
                        dC.Driver.PolePositions,
                        dC.Driver.Wins,
                        dC.Driver.Podiumns,
                        dC.Driver.DNF),
                         new ConstructorDto(
                            dC.Constructor.ConstructorId,
                            dC.Constructor.ConstructorRef,
                            dC.Constructor.Name,
                            dC.Constructor.Nationality,
                            dC.Constructor.Url)
                    )).ToList();

                var pagedResult = new PagedResult<GetAllDriversConstructorsResponse>(driversConstructors, result.Value.TotalCount);

                return Ok(pagedResult);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }


        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateDriverConstructorRequest request)
        {
            try
            {
                if (request == null)
                {
                    return new JsonResult(new { hello = "world" });
                }
                else
                {
                    var driver = await _driverService.GetDriverById(request.DriverId);
                    var constructor = await _constructorService.GetConstructorById(request.ConstructorId);

                    if (driver.IsError || constructor.IsError)
                    {
                        return BadRequest(new { erroor = "no driver or constructor" });
                    }

                    var driverConstructor = new DriverConstructor(
                        request.Year,
                        request.DriverId,
                        request.ConstructorId,
                        constructor.Value,
                        driver.Value
                        );

                    var result = await _driverConstructorService.CreateDriverConstructorAsync(driverConstructor);

                    if (result.IsError)
                    {
                        return BadRequest(new { error = result.Errors });
                    }

                    var created = result.Value;

                    var driverDto = new DriverDto(
                            created.Driver.DriverId,
                            created.Driver.DriverRef,
                            created.Driver.DriverNumber,
                            created.Driver.DriverCode,
                            created.Driver.DriverForename,
                            created.Driver.DriverSurname,
                            created.Driver.DateOfBirth,
                            created.Driver.Nationality,
                            created.Driver.WikipediaUrl,
                            created.Driver.PolePositions,
                            created.Driver.Wins,
                            created.Driver.Podiumns,
                            created.Driver.DNF
                        );

                    var constructorDto = new ConstructorDto(
                            created.Constructor.ConstructorId,
                            created.Constructor.ConstructorRef,
                            created.Constructor.Name,
                            created.Constructor.Nationality,
                            created.Constructor.Url
                        );

                    var response = new DriverConstructorResponse(
                            created.Year, created.ConstructorId, created.DriverId, driverDto, constructorDto
                        );

                    return new JsonResult(response);
                }
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }


        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult<UpdateDriverConstructorResponse>> Put([FromBody] UpdateDriverConstructorRequest body, [FromQuery] UpdateDriverConstructorRequest query)
        {
            try
            {
                if (body == null || query == null)
                {
                    return BadRequest(new { error = "body or query can't be null." });
                }

                var result = await _driverConstructorService.UpdateDriverConstructorAsync(query, body);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });
                }

                var driverConstructor = new UpdateDriverConstructorRequest(result.Value.Year, result.Value.ConstructorId, result.Value.DriverId);

                return Ok(driverConstructor);

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] DeleteDriverConstructorRequest queryParams)
        {
            try
            {
                var result = await _driverConstructorService.DeleteDriverConstructorAsync(queryParams);

                if (result.IsError)
                {
                    return BadRequest(new { error = result.Errors });
                }

                return Ok(queryParams);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }
    }
}
