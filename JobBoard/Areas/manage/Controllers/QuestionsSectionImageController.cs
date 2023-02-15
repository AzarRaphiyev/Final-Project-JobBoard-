using JobBoard.Helpers;
using JobBoard.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
	public class QuestionsSectionImageController : Controller
	{
		private readonly JobBoardContext jobBoardContext;
		private readonly IWebHostEnvironment webHostEnvironment;

		public QuestionsSectionImageController(JobBoardContext jobBoardContext,IWebHostEnvironment webHostEnvironment)
		{
			this.jobBoardContext = jobBoardContext;
			this.webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			List<QuestionsSectionImage> images = jobBoardContext.questionsSectionImages.ToList();
			return View(images);
		}
		public IActionResult Create() { return View(); }
		[HttpPost]
		public IActionResult Create(QuestionsSectionImage questionsSectionImage)
		{
			
			if (!ModelState.IsValid)
			{
				return View();
			}
			if (questionsSectionImage.ImageFile!=null)
			{
				if (questionsSectionImage.ImageFile.ContentType != "image/png" && questionsSectionImage.ImageFile.ContentType != "image/jpeg")
				{
					ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
					return View();
				}
				if (questionsSectionImage.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
					return View();
				}
				questionsSectionImage.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/QuestionsSectionImage", questionsSectionImage.ImageFile);
				
				jobBoardContext.questionsSectionImages.Add(questionsSectionImage);
			}
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Update(int id)
		{
			QuestionsSectionImage questionsSectionImage=jobBoardContext.questionsSectionImages.FirstOrDefault(x => x.Id == id);
			return View(questionsSectionImage);
		}
		[HttpPost]
		public IActionResult Update(QuestionsSectionImage questionsSectionImage)
		{
			if (questionsSectionImage==null) { return View("error"); }
			QuestionsSectionImage extQuestionsSectionImage=jobBoardContext.questionsSectionImages.FirstOrDefault(x=>x.Id== questionsSectionImage.Id);
			if (extQuestionsSectionImage==null) { return View("error"); }
			if (questionsSectionImage.ImageFile != null)
			{
				if (questionsSectionImage.ImageFile.ContentType != "image/png" && questionsSectionImage.ImageFile.ContentType != "image/jpeg")
				{
					ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
					return View();
				}
				if (questionsSectionImage.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
					return View();
				}
				FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/QuestionsSectionImage", extQuestionsSectionImage.Image);
				extQuestionsSectionImage.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/QuestionsSectionImage", questionsSectionImage.ImageFile);
			}
			jobBoardContext.SaveChanges();
			return RedirectToAction("index");

		}
		public IActionResult Delete(int Id)
		{
			QuestionsSectionImage questionsSectionImage = jobBoardContext.questionsSectionImages.FirstOrDefault(x=>x.Id==Id); 
			if (questionsSectionImage==null) return View("error");
			FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/QuestionsSectionImage", questionsSectionImage.Image);
			jobBoardContext.questionsSectionImages.Remove(questionsSectionImage);
			jobBoardContext.SaveChanges();
			return RedirectToAction("index");

		}
	}
}
