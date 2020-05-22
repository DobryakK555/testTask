using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using TestTask.Models;

namespace TestTask.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IMongoDbHelper _mongoDbHelper;
        public IndexModel(IMongoDbHelper mongoDbHelper)
        {
            _mongoDbHelper = mongoDbHelper;
        }

        public void OnGet()
        {
            _mongoDbHelper.CheckTodayWordsDictionary();
        }

        public IActionResult OnPost()
        {
            var word = Request.Form["word"];
            var emailAddress = Request.Form["emailAddress"];

            var todayUserWord = _mongoDbHelper.GetWordOfTheDayForUser(new UserInput
            {
                Word = word,
                EmailAddress = emailAddress,
                PostingTime = DateTime.Today
            });

            return new RedirectToPageResult("/result", new { wordOfTheUser = todayUserWord });
        }
    }
}
