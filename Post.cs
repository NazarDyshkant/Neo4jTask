using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace snmDB.Models
{
    public class Post
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("user_id")]
        public ObjectId UserId { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("created_at")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("reactions")]
        public Dictionary<string, List<ObjectId>> Reactions { get; set; } = new Dictionary<string, List<ObjectId>>();
    }
}
