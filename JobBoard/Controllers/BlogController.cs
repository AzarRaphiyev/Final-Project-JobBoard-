using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace JobBoard.Controllers
{
    public class BlogController : Controller
    {
        private readonly JobBoardContext jobBoardContext;

        public BlogController(JobBoardContext jobBoardContext)
        {
            this.jobBoardContext = jobBoardContext;
        }
        public IActionResult Index()
        {
            List<Blog> blogList = jobBoardContext.blogs.Include(x=>x.Authour).Include(x=>x.Catagory).OrderBy(x=>x.order).ToList();
            return View(blogList);
        }
        public IActionResult Details(int id)
        {
            Blog blog=jobBoardContext.blogs.
                                            Include(x=>x.Authour).
                                            Include(x=>x.Catagory).
                                            FirstOrDefault(x=>x.Id==id);
			if (blog == null)
			{
				return View("Error");
			}
            BlogDitelsViewModel blogDitelsViewModel = new BlogDitelsViewModel
            {
                Blog = blog,
                catagories = jobBoardContext.catagories.ToList(),
            };
           return View(blogDitelsViewModel);
		}
    }
}
