using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace JobBoard.Controllers
{
	public class ContactController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public ContactController(JobBoardContext jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		[Authorize(Roles ="SuperAdmin")]
		public IActionResult Index()
		{
			List<Contact> contacts = jobBoardContext.Contacts.ToList();
			return View(contacts);
		}
		public IActionResult Create()
		{
			ViewBag.InfoBar=jobBoardContext.ContactİnfoBars.Take(1).ToString();
			return View();
		}

		[HttpPost]
		public IActionResult Create(Contact contact)

		{
			ViewBag.InfoBar = jobBoardContext.ContactİnfoBars.Take(1).ToString();
			if (!ModelState.IsValid)
			{
				return View();
			}
			if (contact==null)
			{
				return View("error");
			}
			
			contact.Data = DateTime.Now;
			jobBoardContext.Contacts.Add(contact);
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index","home");
		}

		[Authorize(Roles = "SuperAdmin")]
		public IActionResult Delete(int id)
		{
			Contact deleteContact= jobBoardContext.Contacts.FirstOrDefault(x=>x.Id==id);
			if (deleteContact==null) return View("error");
			jobBoardContext.Contacts.Remove(deleteContact);
			jobBoardContext.SaveChanges();
			return RedirectToAction("index"); 
		}

		public IActionResult Details(int id) 
		{
			Contact contact= jobBoardContext.Contacts.FirstOrDefault(x=>x.Id==id);
			if (contact == null) return View("error");
			return View(contact);
		}

	}
}
