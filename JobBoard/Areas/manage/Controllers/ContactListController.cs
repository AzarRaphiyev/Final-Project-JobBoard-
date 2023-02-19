using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class ContactListController : Controller
    {
        private readonly JobBoardContext jobBoardContext;

        public ContactListController(JobBoardContext jobBoardContext)
        {
            this.jobBoardContext = jobBoardContext;
        }
        public IActionResult Index()
        {
            List<Contact> contacts = jobBoardContext.Contacts.ToList();
            return View(contacts);
        }
        public IActionResult Details(int id)
        {
            Contact contact = jobBoardContext.Contacts.FirstOrDefault(x => x.Id == id);
            if (contact == null) return View("error");
            return View(contact);
        }

		
		public IActionResult Delete(int id)
		{
			Contact deleteContact = jobBoardContext.Contacts.FirstOrDefault(x => x.Id == id);
			if (deleteContact == null) return View("error");
			jobBoardContext.Contacts.Remove(deleteContact);
			jobBoardContext.SaveChanges();
			return RedirectToAction("index");
		}

	}
}
