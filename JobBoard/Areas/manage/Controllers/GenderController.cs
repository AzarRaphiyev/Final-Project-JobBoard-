using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
	public class GenderController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public GenderController(JobBoardContext jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		public IActionResult Index(int page =1)
		{
		
			var query = jobBoardContext.genders.OrderByDescending(x => x.Id).AsQueryable();

			var paginatedlist = PaginationList<Gender>.Create(query, 3, page);
			return View(paginatedlist);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Gender gender)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			jobBoardContext.genders.Add(gender);
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Update(int id)
		{
			Gender gender = jobBoardContext.genders.FirstOrDefault(x => x.Id == id);
			if (gender == null) { return View("error"); }
			return View(gender);
		}
		[HttpPost]
		public IActionResult Update(Gender gender)
		{
			Gender Extgender = jobBoardContext.genders.FirstOrDefault(x => x.Id == gender.Id);
			if (Extgender == null) { return View("error"); }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Extgender.GenderType =gender.GenderType;
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Delete(int id)
		{
			Gender gender = jobBoardContext.genders.FirstOrDefault(x => x.Id == id);
			if (gender == null) { return View("error"); }
			jobBoardContext.genders.Remove(gender);
			jobBoardContext.SaveChanges();
			return Ok();
		}
	}
}
