using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class ServicesController : Controller
    {
        private readonly JobBoardContext jobBoardContext;

        public ServicesController(JobBoardContext jobBoardContext)
        {
            this.jobBoardContext = jobBoardContext;
        }
        public IActionResult Index(int page=1)
        {
            
			var query = jobBoardContext.services.AsQueryable();

			var paginatedlist = PaginationList<ServicesSite>.Create(query, 3, page);
			return View(paginatedlist);
		}
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ServicesSite service)
        {
            if (service==null)
            {
                return View("error");
            }
            jobBoardContext.services.Add(service);
            jobBoardContext.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Update(int id)
        {
            ServicesSite servicesSite = jobBoardContext.services.FirstOrDefault(x => x.Id == id);
            if (servicesSite==null)
            {
                return View("error");
            }
            return View(servicesSite);
        }
        [HttpPost]
        public IActionResult Update(ServicesSite service)
        {
            if (service == null)
            {
                return View("error");
            }
            ServicesSite ExtservicesSite = jobBoardContext.services.FirstOrDefault(x => x.Id == service.Id);
            if (ExtservicesSite == null)
            {
                return View("error");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            ExtservicesSite.Name = service.Name;
            ExtservicesSite.Description = service.Description;
            ExtservicesSite.Icon = service.Icon;
            jobBoardContext.SaveChanges();
            return RedirectToAction("index");
        }
        public IActionResult Delate(int id)
        {
            ServicesSite servicesSite = jobBoardContext.services.FirstOrDefault(x => x.Id == id);
            if (servicesSite==null)
            {
                return View("error");

            }
            jobBoardContext.services.Remove(servicesSite);
            jobBoardContext.SaveChanges();
			return Ok();

		}

    }
}
