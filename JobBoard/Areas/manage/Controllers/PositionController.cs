using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
	public class PositionController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public PositionController(JobBoardContext jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		public IActionResult Index(int page=1)
		{
			var query = jobBoardContext.positions.AsQueryable();

			var paginatedlist = PaginationList<Position>.Create(query, 3, page);
			return View(paginatedlist);
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(Position position) 
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			jobBoardContext.positions.Add(position);
			jobBoardContext.SaveChanges();

		return RedirectToAction("Index");
		}
		public IActionResult Update(int id)
		{
			Position position= jobBoardContext.positions.FirstOrDefault(x=>x.id==id);
			return View(position);
		}
		[HttpPost]
		public IActionResult Update(Position position)
		{
			Position ExsPostion = jobBoardContext.positions.FirstOrDefault(x=> x.id==position.id);
			if (ExsPostion == null) return View("Error");
            if (!ModelState.IsValid)
            {
                return View();
            }
            ExsPostion.Jobname=position.Jobname;
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Delete(int id)
		{
			Position position = jobBoardContext.positions.FirstOrDefault(x => x.id == id);
			if (position==null)
			{
				return View("Error");
			}
			jobBoardContext.positions.Remove(position);
			jobBoardContext.SaveChanges();
			return Ok();
		}
	}
}
