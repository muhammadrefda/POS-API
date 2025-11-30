using Microsoft.EntityFrameworkCore.Storage;
using POS_API.DTOs;
using POS_API.Models;

namespace POS_API.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> CreateAsync(Transaction transaction);

        Task<IEnumerable<Transaction>> GetAllAsync();
        Task<Transaction?> GetByIdAsync(long id);

        Task<IDbContextTransaction> BeginTransactionAsync();

    }
}
