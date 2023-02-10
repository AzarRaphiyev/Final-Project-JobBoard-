﻿

namespace JobBoard.Models
{
	public class AppUser:IdentityUser 
	{
		[Required]
		[StringLength(maximumLength:30,ErrorMessage ="Maksimum 30 simvol ola biler")]
		public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Maksimum 100 simvol ola biler")]
        public string? Image { get; set; }
		[NotMapped]
		public IFormFile ImageFile { get; set; }
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Maksimum 100 simvol ola biler")]
        public string? Role { get; set; }
		

	}
}
