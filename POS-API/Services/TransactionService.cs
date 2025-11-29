using POS_API.DTOs;
using POS_API.Models;
using POS_API.Interfaces;
using Microsoft.EntityFrameworkCore;
using POS_API.Data;

namespace POS_API.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;

        }
        public async Task<Transaction> CreateTransactionAsync(TransactionCreateDto req, long cashierId)
        {
            //transaction db
            using var dbTransaction = await _context.Database.BeginTransactionAsync();

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

                _context.Transactions.Add(newTrans);
                await _context.SaveChangesAsync(); // di save biar dapetin ID Transactionnya

                decimal grandTotal = 0;

                //looping barang belanjaan
                foreach (var item in req.Details)
                {
                    //ngecek produk ada ga?
                    var product = await _context.Products.FindAsync(item.ProductId);
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

                    _context.TransactionDetails.Add(detail);
                }

                //update total harga di header transaction
                newTrans.TotalAmount = grandTotal;
                _context.Transactions.Update(newTrans);

                await _context.SaveChangesAsync();


                await dbTransaction.CommitAsync();


                return newTrans;

            }
            catch (Exception)
            {
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
    }
}
