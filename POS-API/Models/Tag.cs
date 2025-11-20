namespace POS_API.Models
{
    public class Tag : BaseEntity
    {
        public string TagName { get; set; }

        //many to many
        public ICollection<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    }
}
