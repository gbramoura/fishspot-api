using FishSpotApi.Core.Services;
using FishSpotApi.Domain.Exception;
using FishSpotApi.Domain.Http;
using FishSpotApi.Domain.Http.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FishSpotApi.Application.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(UserService userService) : ControllerBase
{
    [HttpPost("register/")]
    [AllowAnonymous]
    public ActionResult<DefaultResponse> Register([FromBody] UserRegisterRequest body)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "Don't authorized"
        };

        try
        {
            if (userService.IsUniqueEmail(body.Email))
            {
                http.Message = "E-mail already in use";
                return StatusCode(http.Code, http);
            }

            userService.RegisterUser(body);

            http.Code = StatusCodes.Status201Created;
            http.Message = "User registered successfully";
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

    [HttpPost("login/")]
    [AllowAnonymous]
    public ActionResult<DefaultResponse> Login([FromBody] UserLoginRequest body)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "Don't authorized"
        };

        try
        {
            var userResponse = userService.LoginUser(body);

            http.Code = StatusCodes.Status200OK;
            http.Message = "User was correctly authenticated.";
            http.Response = userResponse;

            return StatusCode(http.Code, http);
        }
        catch (UserNotFoundException e)
        {
            http.Message = e.Message;
            return StatusCode(http.Code, http);
        }
        catch (IncorrectPasswordException e)
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

    [HttpPost("is-auth")]
    [Authorize(Roles = "user")]
    public ActionResult<DefaultResponse> IsAuth()
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status200OK,
            Message = "Is authenticated"
        };
        
        return StatusCode(http.Code, http);
    }
    
    [HttpPost("refresh-token/")]
    [AllowAnonymous]
    public ActionResult<DefaultResponse> RefreshToken([FromBody] RefreshTokenRequest body)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "Don't authorized"
        };

        try
        {
            var refreshResponse = userService.RefreshToken(body);

            http.Code = StatusCodes.Status200OK;
            http.Message = "Token refreshed";
            http.Response = refreshResponse;

            return StatusCode(http.Code, http);
        }
        catch (RefreshTokenInvalidException e)
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

    [HttpPost("recover-password/")]
    [AllowAnonymous]
    public ActionResult<DefaultResponse> RecoverPassword([FromBody] RecoverPasswordRequest payload)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "Don't authorized."
        };

        try
        {
            userService.GenerateRecoverToken(payload);

            http.Code = StatusCodes.Status200OK;
            http.Message = "A validation token was sent to your e-mail";
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
            http.Message = "Internal Server Error";
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    }

    [HttpPost("validate-recover-token/")]
    [AllowAnonymous]
    public ActionResult<DefaultResponse> ValidateRecoverToken([FromBody] RecoverTokenRequest payload)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "Don't authorized."
        };

        try
        {
            if (userService.ValidateRecoverToken(payload))
            {
                http.Message = "The token is valid";
                http.Code = StatusCodes.Status200OK;
                return StatusCode(http.Code, http);
            }
            
            http.Message = "The token is not valid";
            http.Code = StatusCodes.Status200OK;
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
            http.Message = "Internal Server Error";
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    }
    
    [HttpPost("change-password/")]
    [AllowAnonymous]
    public ActionResult<DefaultResponse> ChangePassword([FromBody] ChangePasswordRequest payload)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = "Don't authorized"
        };

        try
        {
            userService.ChangePassword(payload);

            http.Code = StatusCodes.Status200OK;
            http.Message = "Password changed successfully";
            return StatusCode(http.Code, http);
        }
        catch (InvalidRecoverTokenException e)
        {
            http.Message = e.Message;
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
            http.Message = "Internal Server Error";
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    }
}