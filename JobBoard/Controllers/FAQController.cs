﻿using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Controllers
{
	public class FAQController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public FAQController(JobBoardContext jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		public IActionResult Index()
		{
			FAQvViewModels FaqVm = new FAQvViewModels
			{
				Images = jobBoardContext.questionsSectionImages.Take(1).ToList(),
				QuestionsAndAnswers = jobBoardContext.questionsAndAnswers.OrderBy(x => x.order).Take(5).ToList(),
			};
			return View(FaqVm);
		}
	}
}
