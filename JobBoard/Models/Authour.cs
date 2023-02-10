namespace JobBoard.Models
{
    public class Authour
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 50, ErrorMessage = "Can be a maximum of 50 characters")]
        public string Fullname { get; set; }
        [Required]
        [StringLength(maximumLength: 500, ErrorMessage = "Can be a maximum of 500 characters")]
        public string Description { get; set; }
        [StringLength(maximumLength: 50, ErrorMessage = "Can be a maximum of 50 characters")]
        public string? AuthourImage { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public List<Blog>? blogs { get; set; }
    }
}
