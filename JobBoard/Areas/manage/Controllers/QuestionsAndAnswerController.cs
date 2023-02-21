using Microsoft.AspNetCore.Mvc;

namespace JobBoard.Areas.manage.Controllers
{
	[Area("manage")]
	public class QuestionsAndAnswerController : Controller
	{
		private readonly JobBoardContext jobBoardContext;

		public QuestionsAndAnswerController(JobBoardContext jobBoardContext)
		{
			this.jobBoardContext = jobBoardContext;
		}
		public IActionResult Index()
		{
			List<QuestionsAndAnswer> answerList = jobBoardContext.questionsAndAnswers.ToList();
			return View(answerList);
		}
		public IActionResult Create() 
		{
			return View();
		}
		[HttpPost]
		public IActionResult Create(QuestionsAndAnswer questionsAndAnswer) 
		{
			if (!ModelState.IsValid)
			{
				return View();
			}
			if (questionsAndAnswer==null)
			{
				return View("error");
			}
			jobBoardContext.questionsAndAnswers.Add(questionsAndAnswer);
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Update(int id)
		{
			QuestionsAndAnswer questionsAndAnswer=jobBoardContext.questionsAndAnswers.FirstOrDefault(x=>x.Id==id);
			if (questionsAndAnswer==null)
			{
				return View("error");
			}
			return View(questionsAndAnswer);
		}
		[HttpPost]
		public IActionResult Update(QuestionsAndAnswer questionsAnsver)
		{
			if (questionsAnsver == null)
			{
				return View("error");
			}
			QuestionsAndAnswer ExtquestionsAndAnswer=jobBoardContext.questionsAndAnswers.FirstOrDefault(x=>x.Id== questionsAnsver.Id);
			if (ExtquestionsAndAnswer==null)
			{
				return View("error");
			}
            if (!ModelState.IsValid)
            {
                return View();
            }
            ExtquestionsAndAnswer.Question= questionsAnsver.Question;
			ExtquestionsAndAnswer.Answer= questionsAnsver.Answer;
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
		public IActionResult Delete(int id)
		{
			QuestionsAndAnswer answer=jobBoardContext.questionsAndAnswers.FirstOrDefault(x=>x.Id==id);
			if (answer==null)
			{
				return View("error");
			}
			jobBoardContext.questionsAndAnswers.Remove(answer);
			jobBoardContext.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}
