using System.ComponentModel.DataAnnotations;

namespace goonline.DTO
{
    public class TodoRequestDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters.")]
        public string? title { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot be longer than 500 characters.")]
        public string? description { get; set; }

        [Required(ErrorMessage = "Expiry Date is required.")]
        public DateTime expiryDate { get; set; }
    }
}
