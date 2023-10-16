using Microsoft.AspNetCore.Mvc;
using VideoService.Infrastructure.Repositories;
using VideoService.Domain.DTO;
using VideoService.Domain.Models;

namespace VIdeoService.API.Controllers
{
    [Route("api/MongoController")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly VideoRepository _VideoRepository;
        public VideoController(VideoRepository videoRepository)
        {
            _VideoRepository = videoRepository;
        }


        [HttpGet("Video/{id}")]
        [RequestSizeLimit(5368709120)]
        public async Task<IActionResult> GetVideo(string id)
        {
            var Video = await _VideoRepository.GetVideoById(id);
            VideoModel vidm = await _VideoRepository.GetVideoModelById(id);
            string fileName = vidm.Name;

            var file = File(Video, "video/mp4", fileName);

            return file;
        }



        [HttpGet("Model/{id}")]
        [RequestSizeLimit(5368709120)]
        public async Task<IActionResult> GetVideoModel(string id)
        {
            var Video = await _VideoRepository.GetVideoModelById(id);
            return Ok(Video);
        }


        [HttpPost]
        [RequestSizeLimit(5368709120)]
        public async Task<IActionResult> AddVideo([FromForm] VideoModelDTO vidMod)
        {
            
            if (!vidMod.formfile.ContentType.Contains("video") || vidMod.formfile.Length == 0)
            {
                return BadRequest("соси хуй не загружу ничего");
            }

          
            var res = await _VideoRepository.AddVideoToMongo(vidMod);
            return Ok(res.Id);
        }

        /*
        public async Task<IActionResult> UpdateImage(ImageModel image)
        {
            await _ImageRepository.UpdateImage(image);
            return Ok();
        }
        */


        [HttpDelete]
        public async Task<IActionResult> DeleteVideo(string Id)
        {
            _VideoRepository.DeleteVideo(Id);
            return Ok();
        }

    }
}
