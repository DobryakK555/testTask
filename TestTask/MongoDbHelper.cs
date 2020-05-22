using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using TestTask.Models;

namespace TestTask
{
    public class MongoDbHelper : IMongoDbHelper
    {
        private readonly IConfiguration _configuration;
        private string connectionString = null;
        private readonly MongoClient client = null;
        private IMongoDatabase database = null;
        private IMongoCollection<UserInput> collection = null;
        public Dictionary<string, int> TodayWords { get; set; }

        public MongoDbHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("MongoDB");
            client = new MongoClient(connectionString);
            database = client.GetDatabase(_configuration.GetSection("DBName").Value);
            collection = database.GetCollection<UserInput>(_configuration.GetSection("CollectionName").Value);
        }

        public KeyValuePair<string, int> GetWordOfTheDay()
        {
            return TodayWords.Aggregate((l, r) => l.Value > r.Value ? l : r);
        }

        public string GetWordOfTheDayForUser(UserInput input)
        {
            var userTodayPost = CheckTodayUserPost(input.EmailAddress);

            if (userTodayPost != null)
                return userTodayPost.Word;

            collection.InsertOne(input);
            CheckTodayWordsDictionary();
            return input.Word;
        }

        public void CheckTodayWordsDictionary()
        {
            TodayWords = new Dictionary<string, int>();

            var filterBuilder = Builders<UserInput>.Filter;
            var dateFilter = filterBuilder.Gte(x => x.PostingTime, new BsonDateTime(DateTime.Today)) &
                filterBuilder.Lt(x => x.PostingTime, new BsonDateTime(DateTime.Today.AddDays(1)));

            var allInputs = collection.Find(dateFilter).ToList();

            foreach (var input in allInputs)
            {
                if (TodayWords.ContainsKey(input.Word))
                    TodayWords[input.Word]++;
                else
                    TodayWords[input.Word] = 1;
            }
        }
        private UserInput CheckTodayUserPost(string email)
        {
            var filterBuilder = Builders<UserInput>.Filter;
            var dateFilter = filterBuilder.Gte(x => x.PostingTime, new BsonDateTime(DateTime.Today)) &
                filterBuilder.Lt(x => x.PostingTime, new BsonDateTime(DateTime.Today.AddDays(1)));
            var emailFilter = filterBuilder.Eq(x => x.EmailAddress, email);

            var searchResult = collection.Find(dateFilter & emailFilter).ToList().FirstOrDefault();

            return searchResult;
        }
    }
}
