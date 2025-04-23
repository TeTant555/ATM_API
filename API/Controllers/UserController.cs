using BAL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MODEL.ApplicationConfig;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public UserController(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        // [Authorize]
        [HttpGet("GetAllUser")]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                var users = await _unitOfWork.User.GetAll();
                return Ok(new ResponseModel { Message = Messages.Successfully, Status = APIStatus.Successful, Data = users });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }
        [HttpPatch("Withdraw")]
        public async Task<IActionResult> Withdraw(UserRequestDTO user)
        {
            try
            {
                var result = await _userService.Withdraw(user);
                return Ok(new ResponseModel { Message = result.Message, Status = APIStatus.Successful, Data = result.Data });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }
        [HttpPatch("Deposit")]
        public async Task<IActionResult> Deposit(UserRequestDTO user)
        {
            try
            {
                var result = await _userService.Deposit(user);
                return Ok(new ResponseModel { Message = result.Message, Status = APIStatus.Successful, Data = result.Data });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }
        [HttpGet("CheckBalance")]
        public async Task<IActionResult> CheckBalance(Guid userId)
        {
            try
            {
                var result = await _userService.CheckBalance(userId);
                return Ok(new ResponseModel { Message = Messages.Result, Status = APIStatus.Successful, Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }
        [HttpPatch("UpdateUserIsLocked")]
        public async Task<IActionResult> UpdateUserIsLocked(Guid userId)
        {
            try
            {
                var result = await _userService.UpdateUserIsLocked(userId);
                return Ok(new ResponseModel { Message = Messages.UpdateSucess, Status = APIStatus.Successful, Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }

        [HttpGet("GetTransactionByUserId")]
        public async Task<IActionResult> GetTransactionByUserId(Guid userId)
        {
            try
            {
                var result = await _userService.GetTransactionByUserId(userId);
                return Ok(new ResponseModel { Message = Messages.Result, Status = APIStatus.Successful, Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Message = ex.Message, Status = APIStatus.SystemError });
            }
        }
    }
}
