

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
			List<AppUser> Companies= jobBoardContext.Users.Where(x=>x.Role=="Company"&&x.Enabled==true).ToList();
			return View(Companies);
		}
		public IActionResult Delete(string id)
		{
			AppUser user = jobBoardContext.Users.FirstOrDefault(x => x.Id == id);
			if (user == null)
			{
				return View("Error");
			}
		

			jobBoardContext.Remove(user);
			jobBoardContext.SaveChanges();
			return Ok();
		}
	}
}
