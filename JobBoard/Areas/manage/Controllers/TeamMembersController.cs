﻿using JobBoard.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
    [Area("manage")]
    public class TeamController : Controller
    {
        private readonly JobBoardContext jobBoardContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public TeamController(JobBoardContext jobBoardContext, IWebHostEnvironment webHostEnvironment)
        {
            this.jobBoardContext = jobBoardContext;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Team> members = jobBoardContext.JonTeamMembers.Include(x => x.position).ToList();
            return View(members);
        }
        public IActionResult Create()
        {
            ViewBag.Position = jobBoardContext.positions.ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Team member)
        {
            ViewBag.Position = jobBoardContext.positions.ToList();
            if (!ModelState.IsValid) return View();
            if (member.ImageFile != null)
            {
                if (member.ImageFile.ContentType != "image/png" && member.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
                    return View();
                }
                if (member.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
                    return View();
                }
                member.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/team", member.ImageFile);
                jobBoardContext.JonTeamMembers.Add(member);
            }
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int id)
        {
            ViewBag.Position = jobBoardContext.positions.ToList();
            Team teamMember = jobBoardContext.JonTeamMembers.Include(x => x.position).FirstOrDefault(x => x.Id == id);
            return View(teamMember);
        }
        [HttpPost]
        public IActionResult Update(Team teamMember)
        {
            ViewBag.Position = jobBoardContext.positions.ToList();
            Team exstMember = jobBoardContext.JonTeamMembers.Include(x => x.position).FirstOrDefault(x => x.Id == teamMember.Id);
            if (exstMember == null)
            {
                return View("Error");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (teamMember.ImageFile != null)
            {
                if (teamMember.ImageFile.ContentType != "image/png" && teamMember.ImageFile.ContentType != "image/jpeg")
                {
                    ModelState.AddModelError("ImageFile", "But Png, Jpeg and Jpg can be downloaded");
                    return View();
                }
                if (teamMember.ImageFile.Length > 3145728)
                {
                    ModelState.AddModelError("ImageFile", "It cannot be more than 3 MB");
                    return View();
                }
                FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/team", exstMember.Image);
                exstMember.Image = FileManager.SaveFile(webHostEnvironment.WebRootPath, "uploads/team", teamMember.ImageFile);

            }
            exstMember.InstagramUlr = teamMember.InstagramUlr;
            exstMember.LinkedinUrl = teamMember.LinkedinUrl;
            exstMember.FecebookUrl = teamMember.FecebookUrl;
            exstMember.TwitterUrl = teamMember.TwitterUrl;
            exstMember.Description = teamMember.Description;
            exstMember.Order = teamMember.Order;
            exstMember.Fullname = teamMember.Fullname;
            exstMember.PositionId = teamMember.PositionId;
            jobBoardContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(int id)
        {
            Team teamMember = jobBoardContext.JonTeamMembers.FirstOrDefault(x => x.Id == id);
            if (teamMember == null) return View("Error");
            FileManager.DeleteFile(webHostEnvironment.WebRootPath, "uploads/team", teamMember.Image);
            jobBoardContext.JonTeamMembers.Remove(teamMember);
            jobBoardContext.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
