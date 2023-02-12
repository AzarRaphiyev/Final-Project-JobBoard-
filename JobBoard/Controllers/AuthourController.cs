using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
	public class AuthourController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public AuthourController(JobBoardContext  jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Details(int id)
		{
			Authour authour=jobBoardContext.authours.FirstOrDefault(x=>x.Id==id);
			if (authour==null)
			{
				return View("Error");
			}
			AuthourDetailsViewModel authourDetailsVM = new AuthourDetailsViewModel
			{
				Authour = authour,
				RelationBlogs = jobBoardContext.blogs.OrderBy(x => x.order).Where(x => x.AuthourId == id).ToList(),
			};
			return View(authourDetailsVM);
		}
	}
}
