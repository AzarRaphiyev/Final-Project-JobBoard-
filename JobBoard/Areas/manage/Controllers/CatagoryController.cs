﻿using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class CatagoryController : Controller
    {
        private readonly JobBoardContext jobBoardContext;

        public CatagoryController(JobBoardContext jobBoardContext)
        {
            this.jobBoardContext = jobBoardContext;
        }
        public IActionResult Index()
        {
            List<Catagory> catagories= jobBoardContext.catagories.ToList();
            return View(catagories);
        }
      
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Catagory catagory)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            jobBoardContext.catagories.Add(catagory);
            jobBoardContext.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            Catagory catagory = jobBoardContext.catagories.FirstOrDefault(x => x.Id == id);
            return View(catagory);
        }
        [HttpPost]
        public IActionResult Update(Catagory catagory)
        {
            Catagory Exscatagory = jobBoardContext.catagories.FirstOrDefault(x => x.Id == catagory.Id);
            if (Exscatagory == null) return View("Error");
            Exscatagory.CatagoryName = catagory.CatagoryName;
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Catagory catagory = jobBoardContext.catagories.FirstOrDefault(x => x.Id == id);
            if (catagory == null)
            {
                return View("Error");
            }
            jobBoardContext.catagories.Remove(catagory);
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}