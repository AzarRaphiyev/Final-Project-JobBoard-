

using JobBoard.Models;
using Org.BouncyCastle.Utilities.IO;
using System.Drawing;

namespace JobBoard.Controllers
{
	public class HomeController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public HomeController(JobBoardContext jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		public IActionResult Index(string? searchBy = null, string? search = null, int? regionId = 0, int? typeid = 0)
		
		{
			ViewBag.Region = jobBoardContext.Regions.ToList();
			ViewBag.Type = jobBoardContext.jobTypes.ToList();

			var model = new IndexViewModel();

			if (searchBy == "Name")
			{
                var query = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.Gender).Include(x => x.JobRegion).AsQueryable();
                if (search != null)
                {
                    model.jobs = query.Where(s => s.Title.Contains(search)).ToList();
					model.Supports = jobBoardContext.supportsCompany.OrderByDescending(x => x.Id).Take(8).ToList();
					model.reklams = jobBoardContext.reklams.OrderBy(x => x.Order).ToList();
					model.Companies = jobBoardContext.Users.Where(x => x.Role == "Company").Where(x => x.Enabled == true).ToList();
					model.Members = jobBoardContext.Users.Where(x => x.Role == "Member").ToList();
					model.teams = jobBoardContext.JonTeamMembers.ToList();
					model.fullcobs = jobBoardContext.Jobs.ToList();
                }
                if (typeid != null)
                {
                    model.jobs = query.Where(s => s.JobTypeId == typeid).ToList();
                    model.Supports = jobBoardContext.supportsCompany.OrderByDescending(x => x.Id).Take(8).ToList();
                    model.reklams = jobBoardContext.reklams.OrderBy(x => x.Order).ToList();
                    model.Companies = jobBoardContext.Users.Where(x => x.Role == "Company").Where(x => x.Enabled == true).ToList();
                    model.Members = jobBoardContext.Users.Where(x => x.Role == "Member").ToList();
                    model.teams = jobBoardContext.JonTeamMembers.ToList();
                    model.fullcobs = jobBoardContext.Jobs.ToList();
                }
                if (regionId != null)
				{
                    model.jobs = query.Where(s => s.JobRegionId == regionId).ToList();
                    model.Supports = jobBoardContext.supportsCompany.OrderByDescending(x => x.Id).Take(8).ToList();
                    model.reklams = jobBoardContext.reklams.OrderBy(x => x.Order).ToList();
                    model.Companies = jobBoardContext.Users.Where(x => x.Role == "Company").Where(x => x.Enabled == true).ToList();
                    model.Members = jobBoardContext.Users.Where(x => x.Role == "Member").ToList();
                    model.teams = jobBoardContext.JonTeamMembers.ToList();
                    model.fullcobs = jobBoardContext.Jobs.ToList();
                }
                return View(model);
               
			}
			else
			{
				model = new IndexViewModel
				{
					jobs = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.Gender).Include(x => x.JobRegion).ToList(),
					Supports = jobBoardContext.supportsCompany.OrderByDescending(x => x.Id).Take(8).ToList(),
					reklams = jobBoardContext.reklams.OrderBy(x=>x.Order).ToList(),
					Companies = jobBoardContext.Users.Where(x => x.Role == "Company").Where(x => x.Enabled == true).ToList(),
					Members = jobBoardContext.Users.Where(x => x.Role == "Member").ToList(),
					teams = jobBoardContext.JonTeamMembers.ToList(),
					fullcobs = jobBoardContext.Jobs.ToList()
				};
			}
			return View(model);
		}
	

	}
}