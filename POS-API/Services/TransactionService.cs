using POS_API.Data;
using POS_API.DTOs;
using POS_API.Interfaces;
using POS_API.Models;
using Microsoft.EntityFrameworkCore;

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
            // 1. Mulai Database Transaction (PENTING: Biar data konsisten)
            using var dbTransaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 2. Buat Header Transaksi dulu
                var newTrans = new Transaction
                {
                    CustomerId = req.CustomerId,
                    PaymentMethod = req.PaymentMethod,
                    TransactionDate = DateTime.Now, // Sesuai model kamu
                    CreatedBy = cashierId,          // Sesuai model kamu
                    TotalAmount = 0,                // Nanti dihitung belakangan

                    // BaseEntity otomatis isi CreatedAt, jadi gak perlu diisi manual
                };

                _context.Transactions.Add(newTrans);
                await _context.SaveChangesAsync(); // Save buat dapet ID Transaksi

                decimal grandTotal = 0;

                // 3. Looping Barang Belanjaan
                foreach (var item in req.Details)
                {
                    // Cek Produk ada gak?
                    var product = await _context.Products.FindAsync(item.ProductId);
                    if (product == null) throw new Exception($"Product ID {item.ProductId} not found");

                    // Cek Stok Cukup gak?
                    if (product.Stock < item.Qty) throw new Exception($"Stok {product.ProductName} kurang!");

                    // A. Kurangi Stok
                    product.Stock -= item.Qty;

                    // B. Hitung Duit
                    decimal subTotal = product.Price * item.Qty;
                    grandTotal += subTotal;

                    // C. Masukkan ke tabel Detail (Perhatikan nama modelnya)
                    var detail = new TransactionDetail
                    {
                        TransactionId = newTrans.Id, // Link ke Header
                        ProductId = item.ProductId,
                        Quantity = item.Qty,
                        UnitPrice = product.Price,
                        SubTotal = subTotal
                    };

                    _context.TransactionDetails.Add(detail);
                }

                // 4. Update Total Harga di Header
                newTrans.TotalAmount = grandTotal;
                _context.Transactions.Update(newTrans);

                await _context.SaveChangesAsync();

                // 5. Commit (Simpan Permanen)
                await dbTransaction.CommitAsync();

                return newTrans;
            }
            catch (Exception)
            {
                // 6. Rollback (Batalkan semua kalau ada error)
                await dbTransaction.RollbackAsync();
                throw;
            }
        }
    }
}