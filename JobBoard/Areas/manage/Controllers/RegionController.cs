using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class RegionController : Controller
    {
        private readonly JobBoardContext jobBoardContext;

        public RegionController(JobBoardContext jobBoardContext )
        {
            this.jobBoardContext = jobBoardContext;
        }
        public IActionResult Index(int page=1)
        {
           
			var query = jobBoardContext.Regions.OrderByDescending(x => x.Id).AsQueryable();

			var paginatedlist = PaginationList<JobRegion>.Create(query, 3, page);
			return View(paginatedlist);
		}
        public IActionResult Create()
        {
            return View();  
        }
        [HttpPost]
        public IActionResult Create(JobRegion jobRegion) 
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            jobBoardContext.Regions.Add(jobRegion);
            jobBoardContext.SaveChanges();
        return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            JobRegion jobRegion=jobBoardContext.Regions.FirstOrDefault(x=>x.Id==id);
            if (jobRegion==null) { return View("error"); }
            return View (jobRegion);
        }
        [HttpPost]
        public IActionResult Update(JobRegion jobRegion )
        {
            JobRegion ExtRegion = jobBoardContext.Regions.FirstOrDefault(x => x.Id == jobRegion.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (ExtRegion==null) { return View("error"); }
            ExtRegion.Region = jobRegion.Region;
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            JobRegion jobRegion=jobBoardContext.Regions.FirstOrDefault(x=>x.Id==id);
            if (jobRegion==null) { return View("error"); }
            jobBoardContext.Regions.Remove(jobRegion);
            jobBoardContext.SaveChanges();
			return Ok();
		}
    }
}
