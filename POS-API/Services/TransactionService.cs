using POS_API.DTOs;
using POS_API.Models;
using POS_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using POS_API.Data;

namespace POS_API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepo;
        private readonly IProductRepository _productRepo;


        public TransactionService(ITransactionRepository transactionRepo, IProductRepository productRepo)
        {
            _transactionRepo = transactionRepo;
            _productRepo = productRepo;

        }

        public async Task<IEnumerable<TransactionResponseDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepo.GetAllAsync();

            return transactions.Select(t => new TransactionResponseDto
            {
                TransactionId = t.Id,
                TransactionDate = t.TransactionDate,
                CustomerId = t.CustomerId,
                PaymentMethod = t.PaymentMethod,
                TotalAmount = t.TotalAmount,
                InvoiceNumber = $"INV/{t.TransactionDate:yyyyMMdd}/{t.Id}",
                Details = t.TransactionDetail.Select(d => new TransactionDetailResponseDto
                {
                    ProductId = d.ProductId,
                    ProductName = d.Product.ProductName ?? "Unknown",
                    Qty = d.Quantity,
                    UnitPrice = d.UnitPrice,
                    SubTotal = d.SubTotal
                }).ToList()
            });
        }



        public async Task<Transaction> CreateTransactionAsync(TransactionCreateDto req, long cashierId)
        {
            using var dbTransaction = await _transactionRepo.BeginTransactionAsync();

            try
            {
                // header transaction
                var newTrans = new Transaction
                {
                    CustomerId = req.CustomerId,
                    PaymentMethod = req.PaymentMethod,
                    TransactionDate = DateTime.Now,
                    CreatedBy = cashierId,
                    TotalAmount = 0, // nanti kita hitung belakangan
                };

                decimal grandTotal = 0;

                //looping barang belanjaan
                foreach (var item in req.Details)
                {
                    //ngecek produk ada ga?
                    var product = await _productRepo.GetByIdAsync(item.ProductId);
                    if (product == null)
                    {
                        throw new Exception($" Product ID {item.ProductId} Not Found");
                    }

                    //ngecek stock cukup ga
                    if( product.Stock < item.Qty)
                    {
                        throw new Exception($" Stock {product.ProductName} kurang!");
                    }

                    product.Stock -= item.Qty;

                    decimal subTotal = product.Price * item.Qty;
                    grandTotal += subTotal;

                    //detail transaction
                    var detail = new TransactionDetail
                    {
                        TransactionId = newTrans.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Qty,
                        UnitPrice = product.Price,
                        SubTotal = subTotal
                    };
                }

                //update total harga di header transaction
                newTrans.TotalAmount = grandTotal;

                var savedTrx = await _transactionRepo.CreateAsync(newTrans);

                await dbTransaction.CommitAsync();


                return savedTrx;

            }
            catch (Exception)
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
    }
}
