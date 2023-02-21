using JobBoard.Helpers;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
	public class JobController : Controller
	{
		private readonly JobBoardContext jobBoardContext;
		private readonly IWebHostEnvironment webHostEnvironment;

		public JobController(JobBoardContext jobBoardContext,IWebHostEnvironment webHostEnvironment)
		{
			this.jobBoardContext = jobBoardContext;
			this.webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Create() 
		
		{
			ViewBag.Region = jobBoardContext.Regions.ToList();
			ViewBag.Type = jobBoardContext.jobTypes.ToList();
			ViewBag.Genre = jobBoardContext.genders.ToList();
			return View();
		}

		[HttpPost]
		public IActionResult Create(Job job)
		{
            ViewBag.Region = jobBoardContext.Regions.ToList();
            ViewBag.Type = jobBoardContext.jobTypes.ToList();
			ViewBag.Genre = jobBoardContext.genders.ToList();

            if (!ModelState.IsValid)
			{
				return View();
			}
			if (job.MaxSalary<0)
			{
				ModelState.AddModelError("MaxSalary", "Menfi ola bilmez");
				return View();

			}
			if (job.ImageFile!=null)
			{

				if (job.ImageFile.ContentType != "image/png" && job.ImageFile.ContentType != "image/jpeg")
				{
					ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
					return View();
				}
				if (job.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
					return View();
				}
				job.Image=FileManager.SaveFile(webHostEnvironment.WebRootPath,"uploads/job",job.ImageFile);
				job.PublishedOn= DateTime.Now;
				job.Company = jobBoardContext.companies.FirstOrDefault(x => x.UserName == User.Identity.Name);
				jobBoardContext.Jobs.Add(job);
			}
			jobBoardContext.SaveChanges();
			return RedirectToAction("Create");
		}
	}
}
