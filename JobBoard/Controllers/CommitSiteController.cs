﻿using JobBoard.Helpers;
using JobBoard.Models;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
    public class CommitSiteController : Controller
    {
        private readonly JobBoardContext jobBoardcontext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CommitSiteController(JobBoardContext jobBoardcontext, IWebHostEnvironment webHostEnvironment)
        {
            this.jobBoardcontext = jobBoardcontext;
            this.webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles ="SuperAdmin,Admin")]
        public IActionResult Index()
        {
            List<CommentSite>   comments = jobBoardcontext.commentSites.ToList();
            return View(comments);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CommentSite commentSite)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (commentSite == null)
            {
                return View("error");
            }
            if (commentSite.ImageFile != null)
            {

                if (commentSite.ImageFile.ContentType != "image/png" && commentSite.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
                    return View();
                }
                if (commentSite.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "The size cannot exceed 3 MB");
                    return View();
                }
                commentSite.Commentatorİmage = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/commentsite", commentSite.ImageFile);
                commentSite.Data = DateTime.Now;
                jobBoardcontext.commentSites.Add(commentSite);
            }
            jobBoardcontext.SaveChanges();
            return RedirectToAction("index","Home");
        }
		[Authorize(Roles = "SuperAdmin,Admin")]
		public IActionResult Update(int id)
        {
            CommentSite commentSite = jobBoardcontext.commentSites.FirstOrDefault(x => x.Id == id);
            if (commentSite == null)
            {
                return View("error");
            }
            return View(commentSite);
        }
        [HttpPost]
        public IActionResult Update(CommentSite commentSite)
        {
            if (commentSite == null) { return View("error"); }
            CommentSite extCommentSite = jobBoardcontext.commentSites.FirstOrDefault(x => x.Id == commentSite.Id);
            if (extCommentSite == null) { return View("error"); }

            extCommentSite.Data = DateTime.Now;
            extCommentSite.IsFavorıte = commentSite.IsFavorıte;
            jobBoardcontext.SaveChanges();
            return RedirectToAction("Index");
        }
		[Authorize(Roles = "SuperAdmin,Admin")]
		public IActionResult Delete(int id)
        {
            CommentSite commentSite=jobBoardcontext.commentSites.FirstOrDefault(x => x.Id == id);
            if (commentSite == null) { return View("error"); }
            FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/commentsite", commentSite.Commentatorİmage);
            jobBoardcontext.commentSites.Remove(commentSite);
            jobBoardcontext.SaveChanges();
            return RedirectToAction("Index");   
        }
    }
}