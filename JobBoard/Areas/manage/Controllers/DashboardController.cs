

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
