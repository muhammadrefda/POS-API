using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Interfaces;
using System.Security.Claims;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Wajib Login biar kita tau siapa kasirnya
    public class TransactionsApiController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionsApiController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] TransactionCreateDto req)
        {
            try
            {
                // MAGIC MOMENT: Ambil ID Kasir dari Token JWT
                // ClaimTypes.NameIdentifier biasanya dipapping ke "Id" atau "sub"
                // Kalau di AuthService kamu pakai "Id", ganti jadi "Id"
                var userIdString = User.FindFirst("Id")?.Value;

                if (string.IsNullOrEmpty(userIdString))
                {
                    return Unauthorized("Invalid Token: User ID not found");
                }

                long userId = long.Parse(userIdString);

                var result = await _transactionService.CreateTransactionAsync(req, userId);
                return Ok(new { Message = "Transaction success", Data = result });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}