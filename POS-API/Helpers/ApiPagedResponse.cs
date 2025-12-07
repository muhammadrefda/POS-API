namespace POS_API.Helpers
{
    public class ApiPagedResponse<T>
    {
        public bool Status { get; set; }
        public IEnumerable<T>? Data { get; set; }
        public string? Message { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }

        // Sekarang method ini valid
        public static ApiPagedResponse<T> Success(PagedResponse<T> pagedData, string message = "success")
        {
            return new ApiPagedResponse<T>
            {
                Status = true,
                Data = pagedData.Data,
                Message = message,
                TotalRecords = pagedData.TotalRecords,
                TotalPages = pagedData.TotalPages
            };
        }

        // Anda mungkin ingin menambahkan Fail method juga
        public static ApiPagedResponse<T> Fail(string message)
        {
            return new ApiPagedResponse<T>
            {
                Status = false,
                Data = null,
                Message = message,
                TotalRecords = 0,
                TotalPages = 0
            };
        }
    }

}
