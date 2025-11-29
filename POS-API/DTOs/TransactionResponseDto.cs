namespace POS_API.DTOs
{
    public class TransactionResponseDto
    {
        // Data Header Transaksi
        public long TransactionId { get; set; } // ID yang baru terbentuk
        public DateTime TransactionDate { get; set; } // Tanggal transaksi dibuat
        public string InvoiceNumber { get; set; } // (Opsional) Jika ada nomor invoice
        public long CustomerId { get; set; }
        public string PaymentMethod { get; set; }

        // Total Belanja (Penting untuk POS)
        public decimal TotalAmount { get; set; }

        // List Detail Belanjaan (Pakai DTO khusus juga, jangan Entity)
        public List<TransactionDetailResponseDto> Details { get; set; }
    }

    // Kelas tambahan untuk menampung detail item di dalam response
    public class TransactionDetailResponseDto
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; } // Tambahkan Nama Produk biar Frontend tidak bingung
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; } // Harga per barang saat transaksi
        public decimal SubTotal { get; set; } // Qty * UnitPrice
    }
}