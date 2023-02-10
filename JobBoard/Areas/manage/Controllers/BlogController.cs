using JobBoard.Helpers;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
	public class BlogController : Controller
	{
		private readonly JobBoardContext jobBoardContext;
		private readonly IWebHostEnvironment webHostEnvironment;

		public BlogController(JobBoardContext jobBoardContext,IWebHostEnvironment webHostEnvironment)
		{
			this.jobBoardContext = jobBoardContext;
			this.webHostEnvironment = webHostEnvironment;
		}
		public IActionResult Index()
		{
			List<Blog> blogList = jobBoardContext.blogs.Include(x=>x.Authour).Include(x=>x.Catagory).ToList();
			return View(blogList);
		}
		public IActionResult Create() 
		{
			ViewBag.authours=jobBoardContext.authours.ToList();
			ViewBag.catagory=jobBoardContext.catagories.ToList();
			return View();
		}
		[HttpPost]
		public IActionResult Create(Blog blog)
		{
			ViewBag.authours = jobBoardContext.authours.ToList();
			ViewBag.catagory = jobBoardContext.catagories.ToList();
			if(!ModelState.IsValid) { return View(); }
			if (blog.ImageFile!=null)
			{
				if (blog.ImageFile.ContentType != "image/png" && blog.ImageFile.ContentType != "image/jpeg")
				{
					ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
					return View();
				}
				if (blog.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
					return View();
				}
				blog.Image=FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/blog", blog.ImageFile);
				jobBoardContext.blogs.Add(blog);
			}
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Update(int ID)
		{
			ViewBag.authours = jobBoardContext.authours.ToList();
			ViewBag.catagory = jobBoardContext.catagories.ToList();
			Blog blog= jobBoardContext.blogs.FirstOrDefault(x=>x.Id==ID);

			return View(blog);
		}
		[HttpPost]
		public IActionResult Update(Blog blog)
		{
			ViewBag.authours = jobBoardContext.authours.ToList();
			ViewBag.catagory = jobBoardContext.catagories.ToList();
			if (!ModelState.IsValid)  return View();
			if (blog==null)
			{
				return View("error");
			}
			Blog extblog=jobBoardContext.blogs.FirstOrDefault(x=>x.Id==blog.Id);
			if (extblog == null) return View("error");
			if (blog.ImageFile != null)
			{
				if (blog.ImageFile.ContentType != "image/png" && blog.ImageFile.ContentType != "image/jpeg")
				{
					ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
					return View();
				}
				if (blog.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
					return View();
				}
				FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/blog", extblog.Image);
				extblog.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/blog", blog.ImageFile);
			}
			extblog.Title = blog.Title;
			extblog.Description = blog.Description;
			extblog.Data= blog.Data;
			extblog.CatagoryId= blog.CatagoryId;
			extblog.AuthourId= blog.AuthourId;
			jobBoardContext.SaveChanges();

			return RedirectToAction("index");
		}
		public IActionResult Delete(int id)
		{
			Blog blog=jobBoardContext.blogs.FirstOrDefault(x=>x.Id==id);
			if (blog == null) return View("error");
			FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/blog", blog.Image);
			jobBoardContext.blogs.Remove(blog);
			jobBoardContext.SaveChanges();
			return RedirectToAction("index");
		}
	}
}
