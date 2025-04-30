using AutoMapper;
using BAL.IServices;
using MODEL.ApplicationConfig;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services;


internal class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper;
    }
    public async Task<bool> UpdateUserIsLocked(Guid userId)
    {
        try
        {
            var user = await _unitOfWork.User.GetById(userId);
            if (user != null)
            {
                user.IsLocked = "Y";
                _unitOfWork.User.Update(user);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception)
        {
            throw;
        }

    }
    public async Task<UserResponseDTO> Withdraw(UserRequestDTO user)
    {
        var item = await _unitOfWork.User.GetById(user.Id);
        if (item == null)
        {
            return new UserResponseDTO
            {
                Message = "User not found.",
                Data = null
            };
        }

        if (user.Amount <= 0)
            return new UserResponseDTO
            {
                Message = "Amount must be greater than zero.",
                Data = null
            };

        if (user.Amount > item.Wallet)
            return new UserResponseDTO
            {
                Message = "Insufficient funds.",
                Data = null
            };

        item.Wallet -= user.Amount;

        var transaction = new Transaction
        {
            UserID = user.Id,
            Amount = user.Amount,
            TransactionDate = DateTime.Now,
            TransactionType = "Withdraw"
        };

        await _unitOfWork.Transaction.Add(transaction);
        _unitOfWork.User.Update(item);
        var result = await _unitOfWork.SaveChangesAsync();

        return result > 0
            ? new UserResponseDTO
            {
                Message = $"Withdrawal successful. New balance: ${item.Wallet}",
                Data = _mapper.Map<UserDTO>(item)
            }
            : new UserResponseDTO
            {
                Message = "Transaction failed to save.",
                Data = null
            };
    }

    public async Task<UserResponseDTO> Deposit(UserRequestDTO user)
    {
        var item = await _unitOfWork.User.GetById(user.Id);
        if (item == null)
        {
            return new UserResponseDTO
            {
                Message = "User not found.",
                Data = null
            };
        }

        if (user.Amount <= 0)
            return new UserResponseDTO
            {
                Message = "Amount must be greater than zero.",
                Data = null
            };
        item.Wallet += user.Amount;

        var transaction = new Transaction
        {
            UserID = user.Id,
            Amount = user.Amount,
            TransactionDate = DateTime.Now,
            TransactionType = "Deposit"
        };

        await _unitOfWork.Transaction.Add(transaction);
        _unitOfWork.User.Update(item);
        var result = await _unitOfWork.SaveChangesAsync();

        return result > 0
            ? new UserResponseDTO
            {
                Message = $"Deposit successful. New balance: ${item.Wallet}",
                Data = _mapper.Map<UserDTO>(item)
            }
            : new UserResponseDTO
            {
                Message = "Transaction failed to save.",
                Data = null
            };
    }


    public async Task<decimal?> CheckBalance(Guid userId)
    {
        try
        {
            var user = await _unitOfWork.User.GetById(userId);
            return user?.Wallet;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<List<Transaction>> GetTransactionByUserId(Guid userId)
    {
        try
        {
            var transactions = await _unitOfWork.Transaction.GetByCondition(x => x.UserID == userId);

            if (transactions == null || !transactions.Any())
            {
                throw new Exception("No transactions found for this user.");
            }

            return transactions.ToList();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<UserIncomeOutcomeDTO> GetIncomeOutcomebyUserId(Guid userId)
    {
        try
        {
            var transactions = await _unitOfWork.Transaction.GetByCondition(x => x.UserID == userId);
            if (transactions == null || !transactions.Any())
            {
                throw new Exception("No transactions found for this user.");
            }
            var income = transactions.Where(t => t.TransactionType == "Deposit").Sum(t => t.Amount);
            var outcome = transactions.Where(t => t.TransactionType == "Withdraw").Sum(t => t.Amount);
            return
                new UserIncomeOutcomeDTO
                {
                    Income = income,
                    Outcome = outcome
                };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
