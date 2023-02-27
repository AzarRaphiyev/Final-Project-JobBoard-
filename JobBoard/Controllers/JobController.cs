using JobBoard.Helpers;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Drawing;

namespace JobBoard.Controllers
{
    public class JobController : Controller
    {
        private readonly JobBoardContext jobBoardContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public JobController(JobBoardContext jobBoardContext, IWebHostEnvironment webHostEnvironment)
        {
            this.jobBoardContext = jobBoardContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        #region index
        public IActionResult Index(string? searchBy = null, string? search = null, int? RegionId = null, int? TypeId = null, int page = 1)
        {

            ViewBag.Region = jobBoardContext.Regions.ToList();
            ViewBag.Type = jobBoardContext.jobTypes.ToList();

            var model = new JobsViewModel();

            if (searchBy == "Name")
            {
                var query = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.Gender).Include(x => x.JobRegion).AsQueryable();
                if (search != null)
                {
                    model.jobs = query.Where(s => s.Title.Contains(search)).ToList();
                    model.paginatedlist = PaginationList<Job>.Create(query.Where(s => s.Title.Contains(search)), 7, page);
                }
                if (TypeId != null)
                {
                    model.jobs = query.Where(s => s.JobTypeId == TypeId).ToList();
                    model.paginatedlist = PaginationList<Job>.Create(query.Where(s => s.JobTypeId == TypeId), 7, page);
                }
                if (RegionId != null)
                {
                    model.jobs = query.Where(s => s.JobRegionId == RegionId).ToList();
                    model.paginatedlist = PaginationList<Job>.Create(query.Where(s => s.JobRegionId == RegionId), 7, page);
                    return View(model);
                    //model = new JobsViewModel
                    //{


                    //    //.Where(p => p.Title.StartsWith(search != null ? search : null!))
                    //    //.Where(p => p.JobRegionId == RegionId != null ? RegionId:)
                    //    //.Where(p => p.JobTypeId == TypeId)
                    //   ///* Where(x => x.JobRegionId == RegionId).Where(x => x.JobTypeId == TypeId || TypeId == 0)*/.ToList(),
                    //};
                }
                return View(model);
            }
                else
                {
                var query = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.Gender).Include(x => x.JobRegion).AsQueryable();
                model = new JobsViewModel
                    {
                        jobs = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.Gender).Include(x => x.JobRegion).ToList(),
                        paginatedlist = PaginationList<Job>.Create(query, 7, page),

                    };
                }
                return View(model);
        }

        #endregion


        #region Create

        [HttpGet]
        public IActionResult Create()

        {
            ViewBag.Region = jobBoardContext.Regions.ToList();
            ViewBag.Type = jobBoardContext.jobTypes.ToList();
            ViewBag.Genre = jobBoardContext.genders.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Job job)
        {
            ViewBag.Region = jobBoardContext.Regions.ToList();
            ViewBag.Type = jobBoardContext.jobTypes.ToList();
            ViewBag.Genre = jobBoardContext.genders.ToList();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (job.MaxSalary < 0)
            {
                ModelState.AddModelError("MaxSalary", "Menfi ola bilmez");
                return View();

            }
            if (job.ImageFile != null)
            {

                if (job.ImageFile.ContentType != "image/png" && job.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
                    return View();
                }
                if (job.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
                    return View();
                }
                job.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/job", job.ImageFile);
                job.PublishedOn = DateTime.Now;
                job.Company = jobBoardContext.companies.FirstOrDefault(x => x.UserName == User.Identity.Name);
                jobBoardContext.Jobs.Add(job);
            }
            jobBoardContext.SaveChanges();
            return RedirectToAction("index", "home");
        }
        #endregion

        #region Update

        [HttpGet]
        public IActionResult Update(int id)
        {
            ViewBag.Region = jobBoardContext.Regions.ToList();
            ViewBag.Type = jobBoardContext.jobTypes.ToList();
            ViewBag.Genre = jobBoardContext.genders.ToList();
            Job job=jobBoardContext.Jobs.Include(x=>x.JobType).Include(x => x.JobRegion).Include(x => x.Gender).FirstOrDefault(x => x.Id == id);
            if (job == null) { return View("error"); }
            return View(job);
        }
        [HttpPost]
        public IActionResult Update (Job UpdateJob) 
        {
            ViewBag.Region = jobBoardContext.Regions.ToList();
            ViewBag.Type = jobBoardContext.jobTypes.ToList();
            ViewBag.Genre = jobBoardContext.genders.ToList();
            Job ExtJob = jobBoardContext.Jobs.FirstOrDefault(x=>x.Id == UpdateJob.Id);
            if (ExtJob == null) { return View("error"); }
            if (UpdateJob.ImageFile!=null)
            {
                if (UpdateJob.ImageFile.ContentType != "image/png" && UpdateJob.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
                    return View();
                }
                if (UpdateJob.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
                    return View();
                }
                FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/job", ExtJob.Image);
                ExtJob.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/job", UpdateJob.ImageFile);
            }
            ExtJob.Title= UpdateJob.Title;
            ExtJob.Description= UpdateJob.Description;
            ExtJob.ApplicationDeadline= UpdateJob.ApplicationDeadline;
            ExtJob.JobTypeId= UpdateJob.JobTypeId;
            ExtJob.EduExperience= UpdateJob.EduExperience;
            ExtJob.Vacancy= UpdateJob.Vacancy;
            ExtJob.Order= UpdateJob.Order;
            ExtJob.JobRegionId= UpdateJob.JobRegionId;
            ExtJob.Responsibilities= UpdateJob.Responsibilities;
            ExtJob.Experince= UpdateJob.Experince;  
            ExtJob.GenderId= UpdateJob.GenderId;
            ExtJob.MaxSalary= UpdateJob.MaxSalary;
            ExtJob.MinSalary= UpdateJob.MinSalary;
            ExtJob.OutherBenifits=UpdateJob.OutherBenifits;
            jobBoardContext.SaveChanges();
            return RedirectToAction("index","home");
        }
        #endregion

        #region Delete
        public IActionResult Delete(int id)
        {
            Job job = jobBoardContext.Jobs.FirstOrDefault(x => x.Id == id);
            if (job == null) { return View("Error"); }
            FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/job", job.Image);
            jobBoardContext.Jobs.Remove(job);
            jobBoardContext.SaveChanges();
            return Ok(job);
        }

        #endregion

        #region Details
        public IActionResult Details(int id)
        {
            Job DetailJob = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.JobRegion).Include(x => x.Gender).FirstOrDefault(x => x.Id == id);

            if (DetailJob == null) { return View("error"); }

            JobDetailsViewModel jobDetailsViewModel = new JobDetailsViewModel
            {
                Job = DetailJob,
                RelationJobs = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.JobRegion).
                Include(x => x.Gender).Where(x => x.Title == DetailJob.Title).Where(x => x.Id != DetailJob.Id).ToList(),
                reklams = jobBoardContext.reklams.ToList()
            };
            return View(jobDetailsViewModel);
        }

        #endregion

        public IActionResult AddtoWhisList(int jonId)
        {

            List<FavoriViewModel> FavoriItems= new List<FavoriViewModel>();
            FavoriViewModel favoriItem = new FavoriViewModel
            {
                JobId = jonId,
            };
            FavoriItems.Add(favoriItem);
            string favoriItemSTR = JsonConvert.SerializeObject(FavoriItems);
            HttpContext.Response.Cookies.Append("Favori", favoriItemSTR);

            return Content("Added");
        }
        public IActionResult GetWhisList()
        {
            List<FavoriViewModel> favoriItems= new List<FavoriViewModel>();
            string favoriItemStr = HttpContext.Request.Cookies["Favorit"];
            if (favoriItemStr != null)
            {
                favoriItems = JsonConvert.DeserializeObject<List<FavoriViewModel>>(favoriItemStr);
            }








            return Json(favoriItems);
        }
        public IActionResult Whisllist() 
        {
            List<Job> jobs = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.Gender).Include(x => x.JobRegion).ToList();



            return View(jobs);
        }
    }
}
