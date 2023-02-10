using JobBoard.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
    public class AuthourController : Controller
    {
        private readonly JobBoardContext jobBoardContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AuthourController(JobBoardContext jobBoardContext,IWebHostEnvironment webHostEnvironment)
        {
            this.jobBoardContext = jobBoardContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Authour> authourList = jobBoardContext.authours.ToList();
            return View(authourList);
        }
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Authour authour)
		{
			if (!ModelState.IsValid) return View();
			if (authour.ImageFile != null)
			{
				if (authour.ImageFile.ContentType != "image/png" && authour.ImageFile.ContentType != "image/jpeg")
				{
					ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
					return View();
				}
				if (authour.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
					return View();
				}
				authour.AuthourImage = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/authour", authour.ImageFile);
				jobBoardContext.authours.Add(authour);
			}
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Update(int id)
		{
			Authour authour = jobBoardContext.authours.FirstOrDefault(x => x.Id == id);
			return View(authour);
		}
		[HttpPost]
		public IActionResult Update(Authour authour)
		{
			Authour exstAuthour = jobBoardContext.authours.FirstOrDefault(x => x.Id == authour.Id);
			if (exstAuthour == null)
			{
				return View("Error");
			}
			if (authour.ImageFile != null)
			{
				if (authour.ImageFile.ContentType != "image/png" && authour.ImageFile.ContentType != "image/jpeg")
				{
					ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
					return View();
				}
				if (authour.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
					return View();
				}
				FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/authour", exstAuthour.AuthourImage);
				exstAuthour.AuthourImage = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/team", authour.ImageFile);

			}
			exstAuthour.Fullname= authour.Fullname;
			exstAuthour.Description= authour.Description;
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Delete(int id)
		{
			Authour authour = jobBoardContext.authours.FirstOrDefault(x => x.Id == id);
			if (authour == null) return View("Error");
			FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/authour", authour.AuthourImage);
			jobBoardContext.authours.Remove(authour);
			jobBoardContext.SaveChanges();

			return RedirectToAction("index");
		}
	}
}
