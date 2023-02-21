﻿namespace JobBoard.Models
{
	public class Job
	{
		public int Id { get; set; }
		public int CompanyId { get; set; }
		public int JobTypeId { get; set; }
		public int JobRegionId { get; set; }
		public int GenderId { get; set; }
		public int Order { get; set; }
		public DateTime? PublishedOn { get; set; }
		public DateTime ApplicationDeadline { get; set; }

		[Required]
		public int MaxSalary { get; set; }

		[Required]
		public int MinSalary { get; set; }
		[Required]
		[Range(0,50,ErrorMessage ="Maksimum 50  olmalidi")]
		public int Experince { get; set; }

		[Required]
		public int	 Vacancy { get; set; }

		[Required]
		[StringLength(maximumLength: 1000, ErrorMessage = "Can be a maximum of 1000 characters")]
		public string Description { get; set; }
		[Required]
		[StringLength(maximumLength: 100, ErrorMessage = "Can be a maximum of 100 characters")]
		public string Title { get; set; }

		[Required]
		[StringLength(maximumLength: 500, ErrorMessage = "Can be a maximum of 500 characters")]
		public string Responsibilities { get; set; }

		[Required]
		[StringLength(maximumLength: 500, ErrorMessage = "Can be a maximum of 500 characters")]
		public string EduExperience { get; set; }
		[Required]
		[StringLength(maximumLength: 500, ErrorMessage = "Can be a maximum of 500 characters")]
		public string OutherBenifits { get; set; }

		

		[StringLength(maximumLength: 500, ErrorMessage = "Can be a maximum of 500 characters")]
		public string? Image { get; set; }
		[NotMapped]
		public IFormFile? ImageFile { get; set; }

		public Company? Company { get; set; }
		public JobType? JobType { get; set; }
		public JobRegion? JobRegion { get; set; }
		public Gender? Gender { get; set; }
	}
}
