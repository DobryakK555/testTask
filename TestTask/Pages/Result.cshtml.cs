using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestTask
{
    public class ResultModel : PageModel
    {
        private readonly IMongoDbHelper _mongoDbHelper;

        public string wordOfTheUser { get; set; }
        public int userWordCount { get; set; }
        public KeyValuePair<string, int> wordOfTheDay { get; set; }
        public Dictionary<string, int> todayWords { get; set; }
        public List<KeyValuePair<string, int>> Statisics = new List<KeyValuePair<string, int>>();

        public ResultModel(IMongoDbHelper mongoDbHelper)
        {
            _mongoDbHelper = mongoDbHelper;
        }

        public void OnGet(string wordOfTheUser)
        {
            this.wordOfTheUser = wordOfTheUser;
            userWordCount = _mongoDbHelper.TodayWords.ContainsKey(this.wordOfTheUser) ? _mongoDbHelper.TodayWords[this.wordOfTheUser] : 0;
            todayWords = _mongoDbHelper.TodayWords;
            wordOfTheDay = _mongoDbHelper.GetWordOfTheDay();

            foreach (var word in todayWords.Keys)
            {
                var distance = LevenshteinDistance.Compute(wordOfTheUser, word);
                if (distance == 1)
                    Statisics.Add(new KeyValuePair<string, int>(word, todayWords[word]));
            }
        }
    }
}