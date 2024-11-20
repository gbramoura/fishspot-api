using FishspotApi.Core.Services;
using FishspotApi.Domain.Exception;
using FishspotApi.Domain.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FishpotApi.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SpotController(SpotService SpotService) : ControllerBase
    {
        private readonly SpotService _spotService = SpotService;

        [HttpPost]
        [Authorize(Roles = "user")]
        public ActionResult<DefaultResponse> CreateSpot()
        {
            var http = new DefaultResponse()
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Dont't authorized"
            };

            try
            {
                // TODO: Register the spot

                http.Code = StatusCodes.Status201Created;
                http.Message = "Spot registred successfuly";
                return StatusCode(http.Code, http);
            }
            catch (Exception E)
            {
                http.Code = StatusCodes.Status500InternalServerError;
                http.Message = "Internal server error";
                http.Error = E.Message;
                return StatusCode(http.Code, http);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "user")]
        public ActionResult<DefaultResponse> GetSpot([FromQuery] string id)
        {
            var http = new DefaultResponse()
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Dont't authorized"
            };

            try
            {
                var spot = _spotService.GetSpot(id);

                http.Code = StatusCodes.Status200OK;
                http.Message = "Spot find";
                http.Response = spot;
                return StatusCode(http.Code, http);
            }
            catch (SpotNotFoundException e)
            {
                http.Message = e.Message;
                return StatusCode(http.Code, http);
            }
            catch (Exception E)
            {
                http.Code = StatusCodes.Status500InternalServerError;
                http.Message = "Internal server error";
                http.Error = E.Message;
                return StatusCode(http.Code, http);
            }
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public ActionResult<DefaultResponse> GetLocations()
        {
            var http = new DefaultResponse()
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Dont't authorized"
            };

            try
            {
                // TODO: Get the current location from query
                var locations = _spotService.GetNearLocations();

                http.Code = StatusCodes.Status200OK;
                http.Message = "Locations find";
                http.Response = locations;
                return StatusCode(http.Code, http);
            }
            catch (Exception E)
            {
                http.Code = StatusCodes.Status500InternalServerError;
                http.Message = "Internal server error";
                http.Error = E.Message;
                return StatusCode(http.Code, http);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "user")]
        public ActionResult<DefaultResponse> DeleteSpot([FromQuery] string id)
        {
            var http = new DefaultResponse()
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Dont't authorized"
            };

            try
            {
                _spotService.DeleteSpot(id);

                http.Code = StatusCodes.Status200OK;
                http.Message = "Spot deleted successfuly";
                return StatusCode(http.Code, http);
            }
            catch (SpotNotFoundException e)
            {
                http.Message = e.Message;
                return StatusCode(http.Code, http);
            }
            catch (Exception E)
            {
                http.Code = StatusCodes.Status500InternalServerError;
                http.Message = "Internal server error";
                http.Error = E.Message;
                return StatusCode(http.Code, http);
            }
        }
    }
}