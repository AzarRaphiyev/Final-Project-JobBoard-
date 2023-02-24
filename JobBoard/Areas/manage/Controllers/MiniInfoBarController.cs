using JobBoard.Helpers;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("MANAGE")]
    public class MiniInfoBarController : Controller
    {
        private readonly JobBoardContext jobBoardContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public MiniInfoBarController(JobBoardContext jobBoardContext,IWebHostEnvironment webHostEnvironment)
        {
            this.jobBoardContext = jobBoardContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<MiniInfoBar> MiniInfoBarList=jobBoardContext.miniInfoBars.ToList();
            return View(MiniInfoBarList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
		public IActionResult Create(MiniInfoBar miniInfoBar)
		{
            if (!ModelState.IsValid) return View();
            if (miniInfoBar.ImageFile!=null)
            {
                if (miniInfoBar.ImageFile.ContentType != "image/png" && miniInfoBar.ImageFile.ContentType != "image/jpeg")
                {
					ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
					return View();
				}
				if (miniInfoBar.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
                    return View();
                }
                miniInfoBar.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/MiniInfoBar", miniInfoBar.ImageFile);
                jobBoardContext.miniInfoBars.Add(miniInfoBar);
            }
            jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
        public IActionResult Update(int id) {
            MiniInfoBar miniInfoBar=jobBoardContext.miniInfoBars.FirstOrDefault(x=>x.Id==id);        
            return View(miniInfoBar);
        }
        [HttpPost]
		public IActionResult Update(MiniInfoBar miniInfoBar)
		{
			MiniInfoBar ExtsminiInfoBar = jobBoardContext.miniInfoBars.FirstOrDefault(x => x.Id == miniInfoBar.Id);
            if (ExtsminiInfoBar != null) return View("Error");
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (miniInfoBar.ImageFile!=null)
            {
				if (miniInfoBar.ImageFile.ContentType != "image/png" && miniInfoBar.ImageFile.ContentType != "image/jpeg")
				{
					ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
					return View();
				}
				if (miniInfoBar.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
					return View();
				}
                FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/MiniInfoBar", ExtsminiInfoBar.Image);
				ExtsminiInfoBar.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/MiniInfoBar", miniInfoBar.ImageFile);

			}
            ExtsminiInfoBar.Title=miniInfoBar.Title;
            ExtsminiInfoBar.Description=miniInfoBar.Description;
            ExtsminiInfoBar.order = miniInfoBar.order;
			return RedirectToAction("index");
		}
        public IActionResult Delete(int id)
        {
            MiniInfoBar miniInfoBar=jobBoardContext.miniInfoBars.FirstOrDefault(x => x.Id == id);
            if (miniInfoBar == null) return View("Error");
            jobBoardContext.miniInfoBars.Remove(miniInfoBar);
            jobBoardContext.SaveChanges();
			return Ok();
		}
	}
}
