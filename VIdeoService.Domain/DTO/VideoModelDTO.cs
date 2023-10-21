using Microsoft.AspNetCore.Http;

namespace VideoService.Domain.DTO
{
    public class VideoModelDTO
    {
        public bool IsConnectedToDocument { get; set; }
        public Guid? DocumentId { get; set; }
        public IFormFile formfile { get; set; }  
    }
}
