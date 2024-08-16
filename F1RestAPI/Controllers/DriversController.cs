using Microsoft.AspNetCore.Mvc;
using F1RestAPI.Contracts.Drivers;
using F1RestAPI.Services.Drivers;
using F1RestAPI.Models;
using F1RestAPI.Contracts.DriversConstructors;
using F1RestAPI.Contracts.Pagination;
using F1RestAPI.Contracts.Constructors;
using Microsoft.AspNetCore.Authorization;

namespace DriversApi.Controllers
{

    [ApiController]
    [Route("api/drivers")]
    public class DriversController : ControllerBase
    {
        private readonly IDriverService _driverService;
        public DriversController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<PagedResult<DriverResponse>>> GetAllDrivers([FromQuery] GetAllDriversRequest query)
        {
            try
            {
                var result = await _driverService.GetAllDriversAsync(query.Page ?? 1, query.Pagesize ?? 20);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });
                }

                var drivers = result.Value.Items.Select(driver => new DriverResponse(driver.DriverId,
                  driver.DriverRef,
                  driver.DriverNumber,
                  driver.DriverCode,
                  driver.DriverForename,
                  driver.DriverSurname,
                  driver.DateOfBirth,
                  driver.Nationality,
                  driver.WikipediaUrl,
                  driver.PolePositions,
                  driver.Wins,
                  driver.Podiumns,
                  driver.DNF
                )).ToList();

                var pagedResult = new PagedResult<DriverResponse>(drivers, result.Value.TotalCount);

                return Ok(pagedResult);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }

        [HttpGet("byparams")]
        public async Task<ActionResult<PagedResult<DriverResponse>>> GetAllDriversByParams([FromQuery] GetAllDriversByParamsRequest query)
        {
            try
            {
                var result = await _driverService.GetAllDriversByParamsAsync(query);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });
                }

                var drivers = result.Value.Items.Select(driver => new DriverResponse(driver.DriverId,
                  driver.DriverRef,
                  driver.DriverNumber,
                  driver.DriverCode,
                  driver.DriverForename,
                  driver.DriverSurname,
                  driver.DateOfBirth,
                  driver.Nationality,
                  driver.WikipediaUrl,
                  driver.PolePositions,
                  driver.Wins,
                  driver.Podiumns,
                  driver.DNF
                )).ToList();

                var pagedResult = new PagedResult<DriverResponse>(drivers, result.Value.TotalCount);

                return Ok(pagedResult);

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DriverByIdResponse>> GetDriverById(int id)
        {
            try
            {
                var result = await _driverService.GetDriverById(id);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });
                }

                var driver = result.Value;

                var response = new DriverByIdResponse(
                  driver.DriverId,
                  driver.DriverRef,
                  driver.DriverNumber,
                  driver.DriverCode,
                  driver.DriverForename,
                  driver.DriverSurname,
                  driver.DateOfBirth,
                  driver.Nationality,
                  driver.WikipediaUrl,
                  driver.PolePositions,
                  driver.Wins,
                  driver.Podiumns,
                  driver.DNF,
                  driver.DriverConstructors?.Select(dC => new DriverConstructorWithConsctructorDto(
                    dC.Year,
                    dC.ConstructorId,
                    dC.DriverId,
                    new ConstructorDto(
                      dC.Constructor.ConstructorId,
                      dC.Constructor.ConstructorRef,
                      dC.Constructor.Name,
                      dC.Constructor.Nationality,
                      dC.Constructor.Url)
                  ))
                );
                return Ok(response);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred."); ;
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateDriverRequest request)
        {
            try
            {
                var driver = new Driver(
              0,
              request.DriverRef,
              request.DriverNumber,
              request.DriverCode,
              request.DriverForename,
              request.DriverSurname,
              request.DateOfBirth,
              request.Nationality,
              request.WikipediaUrl,
              request.PolePositions,
              request.Wins,
              request.Podiumns,
              request.DNF
            );

                var result = await _driverService.CreateDriverAsync(driver);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });

                }

                var created = result.Value;

                return new JsonResult(created);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateDriverRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { error = "body can't be null." });
                }

                var result = await _driverService.UpdateDriverAsync(id, request);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });
                }

                return Ok(result.Value);

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete(int id)
        {
            try
            {
                var result = await _driverService.DeleteDriverAsync(id);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });
                }

                return Ok(new { id = id });
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }
    }
}