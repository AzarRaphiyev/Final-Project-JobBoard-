using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
	public class PortfolioCatagoryController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public PortfolioCatagoryController(JobBoardContext jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		public IActionResult Index()
		{
			List<PoerfolioCatagory> poerfolioCatagories = jobBoardContext.poerfolioCatagories.ToList();		
			return View(poerfolioCatagories);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(PoerfolioCatagory catagory)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			jobBoardContext.poerfolioCatagories.Add(catagory);
			jobBoardContext.SaveChanges();

			return RedirectToAction("Index");
		}
		public IActionResult Update(int id)
		{
			PoerfolioCatagory catagory = jobBoardContext.poerfolioCatagories.FirstOrDefault(x => x.Id == id);
			return View(catagory);
		}
		[HttpPost]
		public IActionResult Update(PoerfolioCatagory catagory)
		{
			PoerfolioCatagory Exscatagory = jobBoardContext.poerfolioCatagories.FirstOrDefault(x => x.Id == catagory.Id);
			if (Exscatagory == null) return View("Error");
			Exscatagory.Name = catagory.Name;
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Delete(int id)
		{
			PoerfolioCatagory catagory = jobBoardContext.poerfolioCatagories.FirstOrDefault(x => x.Id == id);
			if (catagory == null)
			{
				return View("Error");
			}
			jobBoardContext.poerfolioCatagories.Remove(catagory);
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
