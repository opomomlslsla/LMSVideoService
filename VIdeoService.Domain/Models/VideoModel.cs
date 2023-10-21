using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoService.Domain.Models
{
    public class VideoModel
    {
        private Guid? documentId;
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public Guid? DocumentId
        {
            get { return documentId; }
            set
            {
                if (IsConnectedToDocument && value != null)
                    documentId = value;
            }
        }
        public bool IsConnectedToDocument { get; set; }

    }
}
