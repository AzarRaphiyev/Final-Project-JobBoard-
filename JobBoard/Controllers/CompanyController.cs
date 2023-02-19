using JobBoard.Helpers;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
	public class CompanyController : Controller
	{
		private readonly JobBoardContext jobBoardContext;
		private readonly IWebHostEnvironment webHostEnvironment;
		private readonly UserManager<AppUser> userManager;

		public CompanyController(JobBoardContext jobBoardContext, IWebHostEnvironment webHostEnvironment,UserManager<AppUser> userManager )
		{
			this.jobBoardContext = jobBoardContext;
			this.webHostEnvironment = webHostEnvironment;
			this.userManager = userManager;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Edit(string username)
		{
			Company company= jobBoardContext.companies.FirstOrDefault(c => c.UserName == username);
			return View(company);
		}
		[HttpPost]
		public IActionResult Edit(Company company) 
		{

			Company ExstCompany= jobBoardContext.companies.FirstOrDefault(x=>x.Id == company.Id);
			if (ExstCompany==null)
			{
				return View("error");
			}

			AppUser appUser= jobBoardContext.Users.FirstOrDefault(x=>x.UserName==company.UserName);
			if (appUser==null) { return View("error"); }


			if (company.ImageFile!=null)
			{
				if (company.ImageFile.ContentType != "image/png" && company.ImageFile.ContentType != "image/jpeg")
				{
					ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
					return View();
				}
				if (company.ImageFile.Length > 3145728)
				{
					ModelState.AddModelError("ImageFile", "The size cannot exceed 3 MB");
					return View();
				}
				FileManager.DeleteFile(webHostEnvironment.WebRootPath,"uploads/users",ExstCompany.Image);
				ExstCompany.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/users", company.ImageFile);
			}

			ExstCompany.Fullname = company.Fullname;
			ExstCompany.Location= company.Location;
			ExstCompany.Description= company.Description;
			ExstCompany.Slogan= company.Slogan;
			ExstCompany.InstagramUrl= company.InstagramUrl;
			ExstCompany.FecebookUrl= company.FecebookUrl;
			ExstCompany.LinkedinUrl= company.LinkedinUrl;
			ExstCompany.TwitterUrl= company.TwitterUrl;
			appUser.FullName = company.Fullname;


			jobBoardContext.SaveChanges();
			return RedirectToAction("Index","Home");
		}
		public async Task< IActionResult> Delete(string Username) 
		{
			Company company = jobBoardContext.companies.FirstOrDefault(x=>x.UserName==Username);
			if (company==null)
			{
				return NotFound();
			}
			if (company.ImageFile!=null) { FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/users", company.Image); }

			AppUser user = await userManager.FindByEmailAsync(company.Email);
			if (user==null) { return NotFound(); }
			jobBoardContext.companies.Remove(company);	
			jobBoardContext.Users.Remove(user);
			jobBoardContext.SaveChanges();

		    return RedirectToAction("Logout","Account");
		}

	}
}
