using FishspotApi.Core.Services;
using FishspotApi.Domain.Exception;
using FishspotApi.Domain.Http;
using FishspotApi.Domain.Http.Request;
using FishspotApi.Domain.Http.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fishspot_api.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register/")]
        [AllowAnonymous]
        public ActionResult<DefaultResponse> Register([FromBody] UserRegister body)
        {
            DefaultResponse http = new DefaultResponse()
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
        public ActionResult<DefaultResponse> Login([FromBody] UserLogin body)
        {
            DefaultResponse http = new DefaultResponse()
            {
                Code = StatusCodes.Status400BadRequest,
                Message = "Dont't authorized."
            };

            try
            {
                var userResponse = _userService.LoginUser(body);

                http.Code = StatusCodes.Status200OK;
                http.Message = "User was correctly authenticated.";
                http.Response = new UserLoginResponse
                {
                    Email = userResponse.Email,
                    Name = userResponse.Name,
                    Token = userResponse.Token,
                    RefreshToken = userResponse.RefreshToken,
                };

                return StatusCode(http.Code, http);
            }
            catch (LoginUserException e)
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
}
