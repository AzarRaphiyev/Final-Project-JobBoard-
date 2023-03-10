using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class ContactInfoBarController : Controller
    {
        private readonly JobBoardContext jobBoardContext;

        public ContactInfoBarController(JobBoardContext jobBoardContext)
        {
            this.jobBoardContext = jobBoardContext;
        }
        public IActionResult Index(int page =1)
        {
          
			var query = jobBoardContext.ContactİnfoBars.AsQueryable();

			var paginatedlist = PaginationList<ContactİnfoBar>.Create(query, 3, page);
			return View(paginatedlist);
		}
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(ContactİnfoBar contactİnfoBar) 
        {
            if(!ModelState.IsValid)  return View();
            if (contactİnfoBar==null)
            {
                return View("error");
            }
            jobBoardContext.ContactİnfoBars.Add(contactİnfoBar);
            jobBoardContext.SaveChanges();

        return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            ContactİnfoBar contactİnfoBar=jobBoardContext.ContactİnfoBars.FirstOrDefault(x=>x.id==id);
            if (contactİnfoBar==null)
            {
                return View("error");
            }
            return View(contactİnfoBar);
        }
        [HttpPost]
        public IActionResult Update(ContactİnfoBar contactİnfoBar) 
        {
            ContactİnfoBar ExtcontactİnfoBar=jobBoardContext.ContactİnfoBars.FirstOrDefault(x=>x.id==contactİnfoBar.id);
            if (ExtcontactİnfoBar==null)
            {
                return View("error");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            ExtcontactİnfoBar.Address = contactİnfoBar.Address;
            ExtcontactİnfoBar.PhoneNumber = contactİnfoBar.PhoneNumber;
            ExtcontactİnfoBar.Email = contactİnfoBar.Email;
            jobBoardContext.SaveChanges();

			return Ok();
		}
    }
}
