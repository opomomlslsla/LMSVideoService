using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VideoService.Domain.Models
{
    public class VideoModel
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public bool IsConnectedToDocument { get; set; }
        private Guid? documentId;
        public Guid? DocumentId
        {
            get { return documentId; }
            set
            {
                if (IsConnectedToDocument)
                {
                    documentId = value;
                }
                else
                    documentId = value;
            }
        }
    }
}
