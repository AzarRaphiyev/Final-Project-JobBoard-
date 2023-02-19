using JobBoard.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class AppUsersController : Controller
    {
        private readonly JobBoardContext jobBoardContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AppUsersController( JobBoardContext jobBoardContext,IWebHostEnvironment webHostEnvironment)
        {
            this.jobBoardContext = jobBoardContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<AppUser> users = jobBoardContext.Users.ToList();
            return View(users);
        }
        public IActionResult Activateuser(string id)
        {
            AppUser appUser=jobBoardContext.Users.FirstOrDefault(x=>x.Id== id);
            if (appUser==null) { return View("error"); }
            appUser.Enabled= true;
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
        


		public IActionResult Delete(string id) 
        {
        AppUser appUser=jobBoardContext.Users.FirstOrDefault(x=>x.Id == id);    
            if (appUser==null)  return View("error");
            FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/users", appUser.Image);
            jobBoardContext.Users.Remove(appUser);
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
