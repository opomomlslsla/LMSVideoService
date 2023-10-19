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
        [BsonId]
        public string Id { get; set; }
        public string Name { get; set; }
        public Guid? DocumentId{ get; private set; }
        public bool IsConnectedToDocument { get; set; }

        public void SetDocementId(Guid? documentId)
        {
            if(IsConnectedToDocument && documentId != null)
            DocumentId = documentId;
        }
    }
}
