using JobBoard.Helpers;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class ReklamController : Controller
    {
        private readonly JobBoardContext jobBoardContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ReklamController(JobBoardContext jobBoardContext,IWebHostEnvironment webHostEnvironment)
        {
            this.jobBoardContext = jobBoardContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Reklam> reklams=jobBoardContext.reklams.OrderByDescending(x=>x.Id).ToList();
            return View(reklams);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(Reklam reklam)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (reklam.ImageFile!=null)
            {
                if (reklam.ImageFile.ContentType != "image/png" && reklam.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
                    return View();
                }
                if (reklam.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "The size cannot exceed 3 MB");
                    return View();
                }
                reklam.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/reklam", reklam.ImageFile);
                jobBoardContext.reklams.Add(reklam);
            }

            jobBoardContext.SaveChanges();
        return RedirectToAction("Index");
        }
        public IActionResult Edit(int id)
        {
            Reklam reklam=jobBoardContext.reklams.FirstOrDefault(x=>x.Id==id);
            if (reklam==null)
            {
                return View("error");
            }
            return View (reklam);
        }
        [HttpPost]
        public IActionResult Edit(Reklam UpdateReklam)
        {
            if (UpdateReklam==null)
            {
                return View("error");
            }
            Reklam EXTreklam = jobBoardContext.reklams.FirstOrDefault(x => x.Id == UpdateReklam.Id);
            if (EXTreklam==null)
            {
                return View("error");
            }
            if (UpdateReklam.ImageFile != null)
            {
                if (UpdateReklam.ImageFile.ContentType != "image/png" && UpdateReklam.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
                    return View();
                }
                if (UpdateReklam.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "The size cannot exceed 3 MB");
                    return View();
                }
                FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/reklam", EXTreklam.Image);
                EXTreklam.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/reklam", UpdateReklam.ImageFile);
            }
            EXTreklam.Tittle = UpdateReklam.Tittle;
            EXTreklam.Description=  UpdateReklam.Description;
            EXTreklam.DeadlineTime= UpdateReklam.DeadlineTime;
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete (int id ) 
        {
            Reklam reklam=jobBoardContext.reklams.FirstOrDefault(x=>x.Id==id);
            if (reklam==null) return View("error");
            FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads.reklam", reklam.Image);
            jobBoardContext.reklams.Remove(reklam);
            jobBoardContext.SaveChanges();
            return Ok();
        }


    }
    
}
