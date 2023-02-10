using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
    public class AboutController : Controller
    {
        private readonly JobBoardContext jobBoardContext;

        public AboutController(JobBoardContext jobBoardContext)
        {
            this.jobBoardContext = jobBoardContext;
        }
        public IActionResult Index()
        {
            AboutViewModel aboutVM = new AboutViewModel
            {
                members = jobBoardContext.members.ToList(),
                companies = jobBoardContext.companies.ToList(),
                TeamMembers=jobBoardContext.JonTeamMembers.Include(x=>x.position).Take(2).OrderBy(x=>x.Order).ToList(),
                miniInfoBars = jobBoardContext.miniInfoBars.OrderBy(x=>x.order).Take(2).ToList(),
            };
            return View(aboutVM);
        }
    }
}
