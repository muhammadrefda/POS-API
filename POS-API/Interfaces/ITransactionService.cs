using POS_API.Helpers;
using POS_API.DTOs;
using POS_API.Models;

namespace POS_API.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransactionAsync(TransactionCreateDto req, long cashierId);
        Task<IEnumerable<TransactionResponseDto>> GetAllTransactionsAsync();
        Task<PagedResponse<TransactionResponseDto>> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm);

        Task<TransactionResponseDto> GetTransactionByidAsync(long id);
    }
}
