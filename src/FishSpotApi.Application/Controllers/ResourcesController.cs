using FishSpotApi.Core.Services;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http;
using FishSpotApi.Domain.Http.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FishSpotApi.Application.Controllers;

[ApiController]
[Route("resources")]
public class ResourcesController(ResourcesService resourcesService) : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize(Roles = "user")]
    public IActionResult GetResource(string id) 
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "Don't authorized"
        };

        try
        {
            var (resource, extension) = resourcesService.GetResource(id);
            return File(resource, $"image/{extension}");
        }
        catch (ImageNotFoundException e)
        {
            http.Message = e.Message;
            return StatusCode(http.Code, http);
        }
        catch (Exception err)
        {
            http.Code = StatusCodes.Status500InternalServerError;
            http.Message = "Internal Server Error";
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
            Message = "Don't authorized"
        };
        
        try
        {
            var resources = resourcesService.AttachSpotResources(request);
                
            http.Message = "The resources were attached to a spot";
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
            http.Message = "Internal Server Error";
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
            Message = "Don't authorized"
        };
        
        try
        {
            resourcesService.DetachSpotResources(request);
                
            http.Message = "The resources were detached to a spot";
            http.Code = StatusCodes.Status200OK;
            return http;
        }
        catch (Exception e) when(e is InvalidImageException or ImageNotFoundException)
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
            http.Message = "Internal Server Error";
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    }
}