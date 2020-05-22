using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace TestTask.Models
{
    public class UserInput
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        public string Word { get; set; }

        public string EmailAddress { get; set; }

        public DateTime PostingTime { get; set; }
    }
}
