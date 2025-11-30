using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;

namespace POS_API.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }


        public async Task<Transaction> CreateAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<IEnumerable<Transaction>> GetAllAsync()
        {
            return await _context.Transactions
                .Include(t => t.Customer)
                .Include(t => t.TransactionDetail)
                .ThenInclude(td => td.Product)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }



    }
}
