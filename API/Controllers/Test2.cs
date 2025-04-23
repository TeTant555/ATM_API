using BAL.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;
using BAL.Services;
using Microsoft.AspNetCore.Authorization;
using Asp.Versioning;

namespace API.Test2;

[Route("api/[controller]")]
[ApiController]
[ApiVersion("2.0")]
public class LoginController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoginService _loginService;
    private readonly IJwtService _jwtService;

    public LoginController(IUnitOfWork unitOfWork, ILoginService loginService, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _loginService = loginService;
        _jwtService = jwtService;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser([FromBody] UserDTO user)
    {
        try
        {
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new ResponseModel
                {
                    Message = "Username and password are required.",
                    Status = APIStatus.Error
                });
            }

            var result = await _loginService.Login(user);

            if (result.Data == null)
            {
                return BadRequest(new ResponseModel
                {
                    Message = result.Message,
                    Status = APIStatus.Error
                });
            }

            // Generate JWT token
            var token = _jwtService.GenerateToken(result.Data.ToString(), user.UserName);

            return Ok(new ResponseModel
            {
                Message = result.Message,
                Status = APIStatus.Successful,
                Data = new { Token = token, User = result.Data }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ResponseModel
            {
                Message = ex.Message,
                Status = APIStatus.SystemError
            });
        }
    }
}
