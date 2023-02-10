

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    [Authorize(Roles ="SuperAdmin")]
    public class AdminTablesController : Controller
    {
        private readonly JobBoardContext jobBoardContext;

        public AdminTablesController(JobBoardContext jobBoardContext)
        {
            this.jobBoardContext = jobBoardContext;
        }
        public IActionResult Index()
        {
            List<AppUser> Admins = jobBoardContext.Users.Where(x=>x.Role=="Admin").ToList();
            return View(Admins);
        }
        public IActionResult Delete(string id) 
        {
            AppUser user= jobBoardContext.Users.FirstOrDefault(x=>x.Id==id);
            if (user==null)
            {
                return View("Error");
            }
            jobBoardContext.Remove(user);
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index"); 
        }
    }
}
