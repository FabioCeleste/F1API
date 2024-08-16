using Microsoft.AspNetCore.Mvc;
using F1RestAPI.Contracts.Constructor;
using F1RestAPI.Services.Constructors;
using F1RestAPI.Models;
using F1RestAPI.Contracts.Constructors;
using F1RestAPI.Contracts.Pagination;
using F1RestAPI.Contracts.DriversConstructors;
using F1RestAPI.Contracts.Drivers;
using Microsoft.AspNetCore.Authorization;

namespace DriversApi.Controllers
{
    [ApiController]
    [Route("api/constructors")]
    public class ConstructorController : ControllerBase
    {
        private readonly IConstructorService _constructorService;
        public ConstructorController(IConstructorService constructorService)
        {
            _constructorService = constructorService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<PagedResult<ConstructorResponse>>> GetAllConstructors([FromQuery] GetAllConstructorsRequest query)
        {
            try
            {
                var result = await _constructorService.GetAllConstructorsAsync(query.Page ?? 1, query.Pagesize ?? 15);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });

                }

                var constructors = result.Value.Items.Select(constructor => new ConstructorResponse(
                    constructor.ConstructorId,
                    constructor.ConstructorRef,
                    constructor.Name,
                    constructor.Nationality,
                    constructor.Url
                )).ToList();

                var pagedResult = new PagedResult<ConstructorResponse>(constructors, result.Value.TotalCount);

                return Ok(pagedResult);

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }

        [HttpGet("byparams")]
        public async Task<ActionResult<PagedResult<ConstructorResponse>>> GetAllConstructorssByParams([FromQuery] GetAllConstructorsByParamsRequest query)
        {
            try
            {
                var result = await _constructorService.GetAllConstructorsByParamsAsync(query);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });
                }

                var constructors = result.Value.Items.Select(constructor => new ConstructorResponse(
                     constructor.ConstructorId,
                     constructor.ConstructorRef,
                     constructor.Name,
                     constructor.Nationality,
                     constructor.Url
                 )).ToList();

                var pagedResult = new PagedResult<ConstructorResponse>(constructors, result.Value.TotalCount);

                return Ok(pagedResult);

            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");

            }


        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ConstructorResponse>> GetConstructorById(int id)
        {
            try
            {
                var result = await _constructorService.GetConstructorById(id);

                if (result.IsError)
                {
                    return BadRequest(new
                    {
                        error = result.Errors
                    });
                }

                var constructor = result.Value;

                var response = new GetConstructorByIdResponse(
                 constructor.ConstructorId,
                 constructor.ConstructorRef,
                 constructor.Name,
                 constructor.Nationality,
                 constructor.Url,
                 constructor.DriverConstructors?.Select(dC => new DriverConstructorWithDriverDto(
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
                        dC.Driver.DNF
                    )
                 ))
                );

                return Ok(response);
            }
            catch (System.Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }

        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post(CreateConstructorRequest request)
        {
            try
            {
                var constructor = new Constructor(
                0,
                request.ConstructorRef,
                request.Name,
                request.Nationality,
                request.Url
                );


                var result = await _constructorService.CreateConstructorAsync(constructor);

                if (result.IsError)
                {
                    return BadRequest(new { error = result.Errors });

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
        public async Task<IActionResult> PutAsync(int id, [FromBody] UpdateConstructorRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { error = "body can't be null." });
                }

                var result = await _constructorService.UpdateConstructorAsync(id, request);

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
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _constructorService.DeleteConstructorAsync(id);

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
