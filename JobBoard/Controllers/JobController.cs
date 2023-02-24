using JobBoard.Helpers;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;

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

        public IActionResult Index(string? searchBy = null, string? search = null, int? RegionId = null, int? TypeId = null)
        {

            ViewBag.Region = jobBoardContext.Regions.ToList();
            ViewBag.Type = jobBoardContext.jobTypes.ToList();

            var model = new JobsViewModel();

            if (searchBy == "Name")
            {
                var query = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.Gender).Include(x => x.JobRegion).AsQueryable();
                if (search!=null)
                {
                    model.jobs = query.Where(s => s.Title.Contains(search)).ToList();
                }
                if (TypeId != null)
                {
                    model.jobs = query.Where(s => s.JobTypeId == TypeId).ToList();
                }
                if (RegionId != null)
                {
                    model.jobs = query.Where(s => s.JobRegionId == RegionId).ToList();
                }
                return View(model);
                //model = new JobsViewModel
                //{


                //    //.Where(p => p.Title.StartsWith(search != null ? search : null!))
                //    //.Where(p => p.JobRegionId == RegionId != null ? RegionId:)
                //    //.Where(p => p.JobTypeId == TypeId)

                //   ///* Where(x => x.JobRegionId == RegionId).Where(x => x.JobTypeId == TypeId || TypeId == 0)*/.ToList(),
                //};
            }
            else
            {
                model = new JobsViewModel
                {
                    jobs = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.Gender).Include(x => x.JobRegion).ToList(),
                };
            }
            return View(model);
        }
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


        public IActionResult Details(int id)
        {
            Job DetailJob = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.JobRegion).Include(x => x.Gender).FirstOrDefault(x => x.Id == id);

            if (DetailJob == null) { return View("error"); }

            JobDetailsViewModel jobDetailsViewModel = new JobDetailsViewModel
            {
                Job = DetailJob,
                RelationJobs = jobBoardContext.Jobs.Include(x => x.Company).Include(x => x.JobType).Include(x => x.JobRegion).
                Include(x => x.Gender).Where(x => x.Title == DetailJob.Title).Where(x => x.Id != DetailJob.Id).ToList(),
            };
            return View(jobDetailsViewModel);
        }
    }
}
