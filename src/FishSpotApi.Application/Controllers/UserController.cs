using FishSpotApi.Core.Extension;
using FishSpotApi.Core.Services;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http;
using FishSpotApi.Domain.Http.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FishSpotApi.Application.Controllers;

[ApiController]
[Route("user")]
public class UserController(SpotService spotService) : ControllerBase
{
    [HttpGet("/spot")]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> GetUserLocations([FromBody] ListRequest listRequest)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "Don't authorized"
        };

        try
        {
            var userid = User?.Identity?.GetUserId();
            var locations = spotService.GetUserLocations(userid ?? string.Empty, listRequest);

            http.Code = StatusCodes.Status200OK;
            http.Message = "Locations find";
            http.Response = locations;
            return StatusCode(http.Code, http);
        }
        catch (Exception e)
        {
            http.Code = StatusCodes.Status500InternalServerError;
            http.Message = "Internal server error";
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    } 
    
    [HttpDelete("/spot/{id}")]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> DeleteSpot([FromQuery] string id)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "Don't authorized"
        };

        try
        {
            var userId = User?.Identity?.GetUserId();
            spotService.DeleteSpot(id, userId ?? string.Empty);

            http.Code = StatusCodes.Status200OK;
            http.Message = "Spot deleted successfully";
            return StatusCode(http.Code, http);
        }
        catch (SpotNotFoundException e)
        {
            http.Message = e.Message;
            return StatusCode(http.Code, http);
        }
        catch (UserNotAuthorizedException e)
        {
            http.Message = e.Message;
            return StatusCode(http.Code, http);
        }
        catch (Exception e)
        {
            http.Code = StatusCodes.Status500InternalServerError;
            http.Message = "Internal server error";
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    }
}