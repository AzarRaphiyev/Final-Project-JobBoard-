using JobBoard.Database;
using JobBoard.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
	public class CommenSiteController : Controller
	{
		private readonly JobBoardContext jobBoardContext;
		private readonly IWebHostEnvironment webHostEnvironment;

		public CommenSiteController(JobBoardContext jobBoardContext,IWebHostEnvironment webHostEnvironment)
		{
			this.jobBoardContext = jobBoardContext;
			this.webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index(int page=1)
		{
			var query = jobBoardContext.commentSites.AsQueryable();

			var paginatedlist = PaginationList<CommentSite>.Create(query, 3, page);
			return View(paginatedlist);
			
		}
		public IActionResult FavoriteComment(int id)
		{
			CommentSite commentSite = jobBoardContext.commentSites.FirstOrDefault(x => x.Id == id);
			if (commentSite == null)
			{
				return View("error");
			}
			if (commentSite.IsFavorıte==false)
			{
			commentSite.IsFavorıte= true;
			}
			else
			{
			commentSite.IsFavorıte = false;
			}

			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
			
		}
		public IActionResult Delete(int id)
		{
			CommentSite commentSite = jobBoardContext.commentSites.FirstOrDefault(x => x.Id == id);
			if (commentSite == null) { return View("error"); }
			FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/commentsite", commentSite.Commentatorİmage);
			jobBoardContext.commentSites.Remove(commentSite);
			jobBoardContext.SaveChanges();
			return Ok();
		}
		public IActionResult Details(int id)
		{
			CommentSite commentSite=jobBoardContext.commentSites.FirstOrDefault(x=>x.Id== id);
			return View (commentSite);
		}


	}
}
