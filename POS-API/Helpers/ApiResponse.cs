namespace POS_API.Helpers
{
    // Kita gunakan 'T' (generik) agar bisa membungkus data apa saja
    // (IEnumerable<Product>, Product, Category, dll.)
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        // --- Konstruktor (Helper) ---

        // Konstruktor untuk SUKSES
        public ApiResponse(T data, string message = "Request successful")
        {
            Success = true;
            Message = message;
            Data = data;
        }

        // Konstruktor untuk GAGAL
        public ApiResponse(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
            Data = default(T); // Data akan null
        }
    }
}