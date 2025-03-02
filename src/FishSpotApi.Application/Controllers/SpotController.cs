using FishSpotApi.Core.Extension;
using FishSpotApi.Core.Services;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http;
using FishSpotApi.Domain.Http.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FishSpotApi.Application.Controllers;

[ApiController]
[Route("spot")]
public class SpotController(SpotService spotService, IStringLocalizer<Resources.Resources> localizer) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> CreateSpot([FromBody] CreateSpotRequest createSpotRequest)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var userid = User?.Identity?.GetUserId();
            var createdSpot = spotService.CreateSpot(createSpotRequest, userid ?? string.Empty);

            http.Code = StatusCodes.Status201Created;
            http.Message = localizer["spot_registered_successfully"];
            http.Response = createdSpot;
            return StatusCode(http.Code, http);
        }
        catch (UserNotFoundException e)
        {
            http.Message = localizer[e.Message];
            return StatusCode(http.Code, http);
        }
        catch (Exception e)
        {
            http.Code = StatusCodes.Status500InternalServerError;
            http.Message = localizer["internal_server_error"];
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> GetSpot(string id)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var spot = spotService.GetSpot(id);

            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["spot_find"];
            http.Response = spot;
            return StatusCode(http.Code, http);
        }
        catch (SpotNotFoundException e)
        {
            http.Message = localizer[e.Message];
            return StatusCode(http.Code, http);
        }
        catch (Exception e)
        {
            http.Code = StatusCodes.Status500InternalServerError;
            http.Message = localizer["internal_server_error"];
            http.Error = e.Message;
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
            Message = localizer["unauthorized"]
        };

        try
        {
            // TODO: Get the current location from query
            var locations = spotService.GetNearLocations();

            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["spot_location_find"];
            http.Response = locations;
            return StatusCode(http.Code, http);
        }
        catch (Exception e)
        {
            http.Code = StatusCodes.Status500InternalServerError;
            http.Message = localizer["internal_server_error"];
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    }

    [HttpGet("by-user")]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> GetUserLocations([FromQuery] ListRequest listRequest)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var userid = User?.Identity?.GetUserId();
            var locations = spotService.GetUserLocations(userid ?? string.Empty, listRequest);

            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["spot_location_find"];
            http.Response = locations;
            return StatusCode(http.Code, http);
        }
        catch (Exception e)
        {
            http.Code = StatusCodes.Status500InternalServerError;
            http.Message = localizer["internal_server_error"];
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    } 
    
    [HttpPut("{id}")]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> UpdateSpot(string id, [FromBody] UpdateSpotRequest updateSpotRequest)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var userId = User?.Identity?.GetUserId();
            spotService.UpdateSpot(id, updateSpotRequest, userId ?? string.Empty);

            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["spot_updated_successfully"];
            return StatusCode(http.Code, http);
        }
        catch (SpotNotFoundException e)
        {
            http.Message = localizer[e.Message];
            return StatusCode(http.Code, http);
        }
        catch (UserNotAuthorizedException e)
        {
            http.Message = localizer[e.Message];
            return StatusCode(http.Code, http);
        }
        catch (Exception e)
        {
            http.Code = StatusCodes.Status500InternalServerError;
            http.Message = localizer["internal_server_error"];
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> DeleteSpot(string id)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var userId = User?.Identity?.GetUserId();
            spotService.DeleteSpot(id, userId ?? string.Empty);

            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["spot_deleted_successfully"];
            return StatusCode(http.Code, http);
        }
        catch (SpotNotFoundException e)
        {
            http.Message = localizer[e.Message];
            return StatusCode(http.Code, http);
        }
        catch (UserNotAuthorizedException e)
        {
            http.Message = localizer[e.Message];
            return StatusCode(http.Code, http);
        }
        catch (Exception e)
        {
            http.Code = StatusCodes.Status500InternalServerError;
            http.Message = localizer["internal_server_error"];
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    }
}