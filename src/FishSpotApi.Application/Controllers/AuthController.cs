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
[Route("auth")]
public class AuthController(UserService userService, IStringLocalizer<FishSpotResource> localizer) : ControllerBase
{
    [HttpPost("register/")]
    [AllowAnonymous]
    public ActionResult<DefaultResponse> Register([FromBody] UserRegisterRequest body)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            if (userService.IsUniqueEmail(body.Email))
            {
                http.Message = localizer["email_in_use"];
                return StatusCode(http.Code, http);
            }

            userService.RegisterUser(body);

            http.Code = StatusCodes.Status201Created;
            http.Message = localizer["user_register_successfully"];
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

    [HttpPost("login/")]
    [AllowAnonymous]
    public ActionResult<DefaultResponse> Login([FromBody] UserLoginRequest body)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            var userResponse = userService.LoginUser(body);

            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["user_authenticated"];
            http.Response = userResponse;

            return StatusCode(http.Code, http);
        }
        catch (UserNotFoundException e)
        {
            http.Message = e.Message; ;
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
            http.Message = localizer["internal_server_error"];
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
            Message = localizer["is_authenticated"]
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
            Message = localizer["unauthorized"]
        };

        try
        {
            var refreshResponse = userService.RefreshToken(body);

            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["token_refreshed"];
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
            http.Message = localizer["internal_server_error"];
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
            Message = localizer["unauthorized"]
        };

        try
        {
            userService.GenerateRecoverToken(payload);

            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["token_email_sent"];
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

    [HttpPost("validate-recover-token/")]
    [AllowAnonymous]
    public ActionResult<DefaultResponse> ValidateRecoverToken([FromBody] RecoverTokenRequest payload)
    {
        var http = new DefaultResponse()
        {
            Code = StatusCodes.Status400BadRequest,
            Message = localizer["unauthorized"]
        };

        try
        {
            if (userService.ValidateRecoverToken(payload))
            {
                http.Message = localizer["token_valid"];
                http.Code = StatusCodes.Status200OK;
                return StatusCode(http.Code, http);
            }

            http.Message = localizer["token_invalid"];
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
            http.Message = localizer["internal_server_error"];
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
            Message = localizer["unauthorized"]
        };

        try
        {
            userService.ChangePassword(payload);

            http.Code = StatusCodes.Status200OK;
            http.Message = localizer["password_changed"];
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
            http.Message = localizer["internal_server_error"];
            http.Error = e.Message;
            return StatusCode(http.Code, http);
        }
    }
}