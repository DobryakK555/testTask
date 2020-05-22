using System.Collections.Generic;
using TestTask.Models;

namespace TestTask
{
    public interface IMongoDbHelper
    {
        string GetWordOfTheDayForUser(UserInput input);

        void CheckTodayWordsDictionary();

        KeyValuePair<string, int> GetWordOfTheDay();

        Dictionary<string, int> TodayWords { get; set; }
    }
}
