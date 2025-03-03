using FishSpotApi.Core.Extension;
using FishSpotApi.Core.Services;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http;
using FishSpotApi.Domain.Http.Request;
using FishSpotApi.Domain.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FishSpotApi.Application.Controllers;

[ApiController]
[Route("user")]
public class UserController(UserService userService, IStringLocalizer<FishSpotResource> localizer) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> GetUser()
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var userid = User?.Identity?.GetUserId();
            var user = userService.GetUser(userid);
            
            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["user_found_successfully"];
            http.Response = user;
            return StatusCode(http.Code, http);
        }
        catch (UserNotFoundException e)
        {
            http.Message = e.Message;
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

    [HttpPut]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> Update(UserUpdateRequest payload)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var userid = User?.Identity?.GetUserId();
            userService.UpdateUser(payload, userid);
            
            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["user_updated_successfully"];
            return StatusCode(http.Code, http);
        }
        catch (UserNotFoundException e)
        {
            http.Message = e.Message;
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