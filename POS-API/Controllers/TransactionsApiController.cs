using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Helpers;
using POS_API.Interfaces;
using System.Transactions;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                var userIdString = User.FindFirst("Id")?.Value;

                if (string.IsNullOrEmpty(userIdString)) {
                    return Unauthorized("Invalid Token: User ID not found");
                }

                long userId = long.Parse(userIdString);

                var result = await _transactionService.CreateTransactionAsync(req, userId);

                var responseDto = new TransactionResponseDto
                {
                    TransactionId = result.Id,
                    TransactionDate = result.TransactionDate,
                    CustomerId = result.CustomerId,
                    PaymentMethod = result.PaymentMethod,
                    TotalAmount = result.TotalAmount, // Asumsi properti ini ada di Entity

                    // Mapping List Details menggunakan LINQ Select
                    Details = result.TransactionDetail.Select(detail => new TransactionDetailResponseDto
                    {
                        ProductId = detail.ProductId,
                        ProductName = detail.Product.ProductName,
                        Qty = detail.Quantity,
                        UnitPrice = detail.UnitPrice,
                        SubTotal = detail.Quantity * detail.UnitPrice
                    }).ToList()
                };

                var response = new ApiResponse<TransactionResponseDto>(responseDto, "Transaction created successfully");
                return StatusCode(201, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var result = await _transactionService.GetAllTransactionsAsync();
                var response = new ApiResponse<IEnumerable<TransactionResponseDto>>(result, "Transactions retrieved successfully");

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = ex.Message
                });
            }
        }


    }
}
