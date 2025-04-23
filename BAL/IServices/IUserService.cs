using MODEL.ApplicationConfig;
using MODEL.DTOs;
using MODEL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.IServices;

public interface IUserService
{
    Task<bool> UpdateUserIsLocked(Guid userId);
    Task<UserResponseDTO> Withdraw(UserRequestDTO userRequest);
    Task<UserResponseDTO> Deposit(UserRequestDTO userRequest);
    Task<decimal?> CheckBalance(Guid  userId);
    Task<List<Transaction>> GetTransactionByUserId(Guid userId);

}
