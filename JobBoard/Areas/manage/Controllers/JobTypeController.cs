﻿using JobBoard.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
	public class JobTypeController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public JobTypeController(JobBoardContext jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		public IActionResult Index(int page=1)
		{
			var query = jobBoardContext.jobTypes.AsQueryable();

			var paginatedlist = PaginationList<JobType>.Create(query, 3, page);
			return View(paginatedlist);
			
		}
		public IActionResult Create()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(JobType jobType)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			jobBoardContext.jobTypes.Add(jobType);
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Update(int id)
		{
			JobType type = jobBoardContext.jobTypes.FirstOrDefault(x => x.Id == id);

			if (type == null) { return View("error"); }
			return View(type);
		}

		[HttpPost]
		public IActionResult Update(JobType jobType )
		{
			JobType Exttype = jobBoardContext.jobTypes.FirstOrDefault(x => x.Id == jobType.Id);


			if (Exttype == null) { return View("error"); }
            if (!ModelState.IsValid)
            {
                return View();
            }
            Exttype.Type = jobType.Type;
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Delete(int id)
		{
			JobType type = jobBoardContext.jobTypes.FirstOrDefault(x => x.Id == id);
			if (type == null) { return View("error"); }
			jobBoardContext.jobTypes.Remove(type);
			jobBoardContext.SaveChanges();
			return Ok();
		}
	}
}
