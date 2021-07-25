using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TaskBook.Models
{
    public class Item
    {
        [BsonId]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Priority { get; set; } = "None";
        public string BgColor { get; set; } = "SlateGray";
    }
}
