using FishspotApi.Core.Services;
using FishspotApi.Domain.Exception;
using FishspotApi.Domain.Http;
using FishspotApi.Domain.Http.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FishpotApi.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(UserService userService) : ControllerBase
    {
        private readonly UserService _userService = userService;

        [HttpPost("register/")]
        [AllowAnonymous]
        public ActionResult<DefaultResponse> Register([FromBody] UserRegisterRequest body)
        {
            var http = new DefaultResponse()
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Dont't authorized"
            };

            try
            {
                if (_userService.IsUniqueEmail(body.Email))
                {
                    http.Message = "E-mail already in use";
                    return StatusCode(http.Code, http);
                }

                _userService.RegisterUser(body);

                http.Code = StatusCodes.Status201Created;
                http.Message = "User registered succesfully";
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

        [HttpPost("login/")]
        [AllowAnonymous]
        public ActionResult<DefaultResponse> Login([FromBody] UserLoginRequest body)
        {
            var http = new DefaultResponse()
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Dont't authorized"
            };

            try
            {
                var userResponse = _userService.LoginUser(body);

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

        [HttpPost("refresh-token/")]
        [AllowAnonymous]
        public ActionResult<DefaultResponse> AtualizarToken([FromBody] RefreshTokenRequest body)
        {
            var http = new DefaultResponse()
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Dont't authorized"
            };

            try
            {
                var refreshResponse = _userService.RefreshToken(body);

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
                Message = "Dont't authorized."
            };

            try
            {
                _userService.GenerateRecoverToken(payload);

                http.Code = StatusCodes.Status200OK;
                http.Message = "A validation token was sent to your e-mail";
                return StatusCode(http.Code, http);
            }
            catch (UserNotFoundException e)
            {
                http.Message = e.Message;
                return StatusCode(http.Code, http);
            }
            catch (Exception E)
            {
                http.Code = StatusCodes.Status500InternalServerError;
                http.Message = "Internal Server Error";
                http.Error = E.Message;
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
                Message = "Dont't authorized"
            };

            try
            {
                _userService.ChangePassword(payload);

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
            catch (Exception E)
            {
                http.Code = StatusCodes.Status500InternalServerError;
                http.Message = "Internal Server Error";
                http.Error = E.Message;
                return StatusCode(http.Code, http);
            }
        }
    }
}