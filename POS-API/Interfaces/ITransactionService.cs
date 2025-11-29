using POS_API.DTOs;
using POS_API.Models;

namespace POS_API.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransactionAsync(TransactionCreateDto req, long cashierId);
    }
}
