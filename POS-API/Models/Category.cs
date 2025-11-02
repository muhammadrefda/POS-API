using System.ComponentModel.DataAnnotations;

namespace POS_API.Models
{
    public class Category : BaseEntity
    {
        [Display(Name = "Nama Kategori")]
        public string CategoryName { get; set; }
        [Display(Name = "Deskripsi")]
        public string? Description { get; set; }
        [Display(Name ="Status")]
        public bool Active { get; set; } = true;
    }
}
