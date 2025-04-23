using AutoMapper;
using BAL.IServices;
using Microsoft.EntityFrameworkCore.Metadata;
using MODEL.DTOs;
using REPOSITORY.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services;

internal class LoginService : ILoginService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public LoginService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }
    public async Task<LoginResponseDTO> Login(UserDTO user)
    {
        try
        {
            if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return new LoginResponseDTO
                {
                    Message = "Username and password are required.",
                    Data = null,
                    Issuccess = false
                };
            }

            var loginUser = await _unitOfWork.User.GetByCondition(x => 
                x.UserName == user.UserName && 
                x.Password == user.Password);

            if (loginUser == null || !loginUser.Any())
            {
                return new LoginResponseDTO
                {
                    Message = "Invalid username or password.",
                    Data = null,
                    Issuccess = false
                };
            }

            var userEntity = loginUser.FirstOrDefault();

            if (userEntity?.IsLocked == "Y")
            {
                return new LoginResponseDTO
                {
                    Message = "User is inactive.",
                    Data = userEntity,
                    Issuccess = false
                };
            }

            return new LoginResponseDTO
            {
                Message = "Login successful.",
                Data = userEntity,
                Issuccess = true
            };
        }
        catch (Exception ex)
        {
            return new LoginResponseDTO
            {
                Message = $"Login error: {ex.Message}",
                Data = null,
                Issuccess = false
            };
        }
    }

}
