using FishSpotApi.Core.Extension;
using FishSpotApi.Core.Services;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http;
using FishSpotApi.Domain.Http.Request;
using FishSpotApi.Domain.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace FishSpotApi.Application.Controllers;

[ApiController]
[Route("resources")]
public class ResourcesController(ResourcesService resourcesService, IStringLocalizer<FishSpotResource> localizer) : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize(Roles = "user")]
    public IActionResult GetResource(string id)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var (resource, extension) = resourcesService.GetResource(id);
            return File(resource, $"image/{extension}");
        }
        catch (ImageNotFoundException e)
        {
            http.Message = localizer[e.Message];
            return StatusCode(http.Code, http);
        }
        catch (Exception err)
        {
            http.Code = StatusCodes.Status500InternalServerError;
            http.Message = localizer["internal_server_error"];
            http.Error = err.Message;
            return StatusCode(http.Code, http);
        }
    }

    [HttpPost("/attach-to-spot")]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> AttachResourcesToSpot([FromForm] AttachResourcesToSpotRequest request)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var resources = resourcesService.AttachSpotResources(request);

            http.Message = localizer["resource_attached"];
            http.Code = StatusCodes.Status200OK;
            http.Response = resources;
            return http;
        }
        catch (SpotNotFoundException e)
        {
            http.Message = e.Message;
            return StatusCode(http.Code, http);
        }
        catch (InvalidImageException e)
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

    [HttpPost("/detach-to-spot")]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> DetachResourcesToSpot([FromForm] DetachResourcesFromSpotRequest request)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            resourcesService.DetachSpotResources(request);

            http.Message = localizer["resource_detached"];
            http.Code = StatusCodes.Status200OK;
            return http;
        }
        catch (Exception e) when (e is InvalidImageException or ImageNotFoundException)
        {
            http.Message = e.Message;
            return StatusCode(http.Code, http);
        }
        catch (SpotNotFoundException e)
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
    
    [HttpPost("/attach-to-user")]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> AttachResourcesToSpot([FromForm] AttachResourceToUserRequest request)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var userId = User?.Identity?.GetUserId();
            var resource = resourcesService.AttachUserResource(request, userId);

            http.Message = localizer["resource_attached_to_user"];
            http.Code = StatusCodes.Status200OK;
            http.Response = resource;
            return http;
        }
        catch (UserNotFoundException e)
        {
            http.Message = e.Message;
            return StatusCode(http.Code, http);
        }
        catch (InvalidImageException e)
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