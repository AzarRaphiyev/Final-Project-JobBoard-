using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class CatagoryController : Controller
    {
        private readonly JobBoardContext jobBoardContext;

        public CatagoryController(JobBoardContext jobBoardContext)
        {
            this.jobBoardContext = jobBoardContext;
        }
        public IActionResult Index(int page = 1)
        {
            var query = jobBoardContext.catagories.AsQueryable();

            var paginatedlist = PaginationList<Catagory>.Create(query, 3, page);
            return View(paginatedlist);
        }	
      
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Catagory catagory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            jobBoardContext.catagories.Add(catagory);
            jobBoardContext.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Catagory catagory = jobBoardContext.catagories.FirstOrDefault(x => x.Id == id);
            return View(catagory);
        }
        [HttpPost]
        public IActionResult Update(Catagory catagory)
        {
            Catagory Exscatagory = jobBoardContext.catagories.FirstOrDefault(x => x.Id == catagory.Id);
            if (Exscatagory == null) return View("Error");
            if (!ModelState.IsValid)
            {
                return View();
            }
            Exscatagory.CatagoryName = catagory.CatagoryName;
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Catagory catagory = jobBoardContext.catagories.FirstOrDefault(x => x.Id == id);
            if (catagory == null)
            {
                return View("Error");
            }
            jobBoardContext.catagories.Remove(catagory);
            jobBoardContext.SaveChanges();
			return Ok();
		}
    }
}
