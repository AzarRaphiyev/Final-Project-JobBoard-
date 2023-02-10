

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CompanyTablesController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public CompanyTablesController(JobBoardContext jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		public IActionResult Index()
		{
			List<AppUser> Companies= jobBoardContext.Users.Where(x=>x.Role=="Company").ToList();
			return View(Companies);
		}
		public IActionResult Delete(string id)
		{
			AppUser user = jobBoardContext.Users.FirstOrDefault(x => x.Id == id);
			if (user == null)
			{
				return View("Error");
			}
			Company company=jobBoardContext.companies.FirstOrDefault(x => x.Fullname==user.FullName);
			jobBoardContext.companies.Remove(company);
			jobBoardContext.Remove(user);
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
