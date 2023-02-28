using JobBoard.Database;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
	public class MyProfilController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public MyProfilController(JobBoardContext jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		public IActionResult Index()
		{
			return View();
		}



		public IActionResult CompanyProfil(string username)
		{
			Company company = jobBoardContext.companies.FirstOrDefault(c => c.UserName == username);
			CompanyViewModel companyViewModel = new CompanyViewModel
			{
				RelationJobs = jobBoardContext.Jobs.Include(x => x.Company).Include(x=>x.JobType).ToList(),
				Company = company
			};

			return View(companyViewModel);
		}





		public IActionResult MemberProfil(string username) 
		{
		AppUser appUser=jobBoardContext.Users.FirstOrDefault(x=>x.UserName == username);	
			if (appUser == null) { return View("error"); }
			return View(appUser);
		}
	}
}
