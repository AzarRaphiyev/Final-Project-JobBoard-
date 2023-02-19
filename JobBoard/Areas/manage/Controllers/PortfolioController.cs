using JobBoard.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class PortfolioController : Controller
    {
        private readonly JobBoardContext jobBoardContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public PortfolioController(JobBoardContext jobBoardContext,IWebHostEnvironment webHostEnvironment)
        {
            this.jobBoardContext = jobBoardContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<PortfolioItem> portfolioItems= jobBoardContext.portfolioItems.Include(x=>x.Team).Include(x=>x.poerfolioCatagories).Include(x=>x.portfolioItemImages).ToList();
            return View(portfolioItems);
        }


        #region Create

        public IActionResult Create()
        {
            ViewBag.PortfolioCatagory=jobBoardContext.poerfolioCatagories.ToList();
            ViewBag.Team=jobBoardContext.JonTeamMembers.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(PortfolioItem portfolioItem)
        {
            ViewBag.PortfolioCatagory = jobBoardContext.poerfolioCatagories.ToList();
            ViewBag.Team = jobBoardContext.JonTeamMembers.ToList();
            if (portfolioItem.PosterImageFile!=null)
            {
                if (portfolioItem.PosterImageFile.ContentType != "image/png" && portfolioItem.PosterImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImageFile", "But Png, Jpeg and Jpg can be downloaded");
                    return View();

                }
                if (portfolioItem.PosterImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("PosterImageFile", "It cannot be more than 3 MB");
                    return View();
                }
                PortfolioItemImages portfolioItemImages = new PortfolioItemImages
                {
                    portfolioItem = portfolioItem,
                    Images = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/portfolio", portfolioItem.PosterImageFile    ),
					IsPoster = true
                };

                jobBoardContext.portfolioItemImages.Add(portfolioItemImages);
            }



            if (portfolioItem.ImageFiles!=null)
            {

                foreach (var item in portfolioItem.ImageFiles)
                {
					if (item.ContentType != "image/png" && item.ContentType != "image/jpeg")
					{
						ModelState.AddModelError("PosterImageFile", "But Png, Jpeg and Jpg can be downloaded");
						return View();

					}
					if (item.Length > 3145728)
					{
						ModelState.AddModelError("PosterImageFile", "It cannot be more than 3 MB");
						return View();
					}
					PortfolioItemImages Images = new PortfolioItemImages
					{
						portfolioItem = portfolioItem,
						Images = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/portfolio", item),
						IsPoster = null
					};
					jobBoardContext.portfolioItemImages.Add(Images);
				}
                
            }
            portfolioItem.YearStarted=DateTime.Now;
            jobBoardContext.portfolioItems.Add(portfolioItem);
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion



        #region Update

        public IActionResult Update(int id)
        {
            ViewBag.PortfolioCatagory = jobBoardContext.poerfolioCatagories.ToList();
            ViewBag.Team = jobBoardContext.JonTeamMembers.ToList();
            PortfolioItem portfolioItem = jobBoardContext.portfolioItems.Include(x=>x.portfolioItemImages).FirstOrDefault(x => x.Id == id);
            if (portfolioItem == null) { return View("error"); }
            return View(portfolioItem);
        }


        [HttpPost]
        public IActionResult Update(PortfolioItem portfolioItem)
        {
            ViewBag.PortfolioCatagory = jobBoardContext.poerfolioCatagories.ToList();
            ViewBag.Team = jobBoardContext.JonTeamMembers.ToList();
            if (portfolioItem == null) { return View("error"); }
            PortfolioItem ExtItem = jobBoardContext.portfolioItems.
                                                                   Include(x => x.Team).
                                                                   Include(x => x.poerfolioCatagories).
                                                                   Include(x => x.portfolioItemImages).
                                                                   FirstOrDefault(x => x.Id == portfolioItem.Id);
            if (ExtItem == null) { return View("error"); }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (portfolioItem.PosterImageFile != null)
            {

                if (portfolioItem.PosterImageFile.ContentType != "image/png" && portfolioItem.PosterImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("PosterImageFile", "But Png, Jpeg and Jpg can be downloaded");
                    return View();

                }
                if (portfolioItem.PosterImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("PosterImageFile", "It cannot be more than 3 MB");
                    return View();
                }
                FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/portfolio", ExtItem.portfolioItemImages.FirstOrDefault(x => x.IsPoster == true).Images);
                PortfolioItemImages portfolioItemImages = new PortfolioItemImages
                {
                    PortfolioItemId = portfolioItem.Id,
                    Images = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/portfolio", portfolioItem.PosterImageFile),
                    IsPoster = true,
                };
                ExtItem.portfolioItemImages.FirstOrDefault(x => x.IsPoster == true).Images = portfolioItemImages.Images;
                
            }
            if (portfolioItem.ImageFiles != null)
            {
                foreach (var ImageFile in portfolioItem.ImageFiles)
                {
                    FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/portfolio", ExtItem.portfolioItemImages.FirstOrDefault(x=>x.IsPoster==null).Images);
                    if (ImageFile.ContentType != "image/png" && ImageFile.ContentType != "image/jpeg")
                    {
                        ModelState.AddModelError("PosterImageFile", "But Png, Jpeg and Jpg can be downloaded");
                        return View();

                    }
                    if (ImageFile.Length > 3145728)
                    {
                        ModelState.AddModelError("PosterImageFile", "It cannot be more than 3 MB");
                        return View();
                    }
                    PortfolioItemImages portfolioItemImages = new PortfolioItemImages
                    {
                        portfolioItem = portfolioItem,
                        Images = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/portfolio", portfolioItem.PosterImageFile),
                        IsPoster = null
                    };
                ExtItem.portfolioItemImages.Add(portfolioItemImages);
                }
            }
            ExtItem.Title = portfolioItem.Title;
            ExtItem.Description = portfolioItem.Description;
            ExtItem.Team = portfolioItem.Team;
            ExtItem.Order = portfolioItem.Order;    
            ExtItem.Client= portfolioItem.Client;
            ExtItem.YearStarted= portfolioItem.YearStarted;
            ExtItem.poerfolioCatagories=portfolioItem.poerfolioCatagories;
            ExtItem.WebUrl=portfolioItem.WebUrl;
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        #region Delete
        public IActionResult Delete(int id)
        {
            PortfolioItem portfolioItem=jobBoardContext.portfolioItems.Include(x=>x.portfolioItemImages).FirstOrDefault(x => x.Id == id);
            if (portfolioItem==null)
            {
                return View("error");
            }
            if (portfolioItem.portfolioItemImages!=null)
            {
                foreach (var item in portfolioItem.portfolioItemImages)
                {
                    FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/portfolio", item.Images);
                }
            }
            jobBoardContext.portfolioItems.Remove(portfolioItem);
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }


        #endregion
    }
}
