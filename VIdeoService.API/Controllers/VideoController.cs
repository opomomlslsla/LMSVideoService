using Microsoft.AspNetCore.Mvc;
using VideoService.Infrastructure.Repositories;
using VideoService.Domain.DTO;
using VideoService.Domain.Models;
using MongoDB.Bson;

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
        public async Task<IActionResult> AddVideo([FromForm] VideoModelDTO videoModelDTO)
        {
            var bob = await VideoTest(videoModelDTO);

            if (!videoModelDTO.formfile.ContentType.Contains("video") || videoModelDTO.formfile.Length == 0 || !bob.IsSuccessStatusCode)
            {
                return BadRequest("некорректный или опасный файл");
            }
             
            var res = await _VideoRepository.AddVideoToMongo(videoModelDTO);
            return Ok($"видео загружено id:{res.Id}");
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

        
        private async Task<HttpResponseMessage> VideoTest(VideoModelDTO vidMod)
        {
            HttpClient client = new HttpClient();
            MultipartFormDataContent content = new MultipartFormDataContent();

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://www.filescan.io/api/scan/file");
            httpRequestMessage.Headers.Add("X-Api-Key", "2rvMHJfeZn_RS65uUzY6kLNRH0dnwxM0C9PJjkgU");

            content.Headers.Add("X-Api-Key", "2rvMHJfeZn_RS65uUzY6kLNRH0dnwxM0C9PJjkgU");

            content.Add(new StreamContent(vidMod.formfile.OpenReadStream()), "file", vidMod.formfile.FileName);
            
            /*
            var pairs = new List<KeyValuePair<string, string>>()
            {
                 new KeyValuePair<string, string>("save_preset", "false"),
                 new KeyValuePair<string, string>("description", ""),
                 new KeyValuePair<string, string>("tags", ""),
                 new KeyValuePair<string, string>("propagate_tags", "false"),
                 new KeyValuePair<string, string>("password", ""),
                 new KeyValuePair<string, string>("is_private", "true"),
                 new KeyValuePair<string, string>("skip_whitelisted", "true"),
                 new KeyValuePair<string, string>("rapid_mode", "true"),
                 new KeyValuePair<string, string>("osint", "true"),
                 new KeyValuePair<string, string>("extended_osint", "true"),
                 new KeyValuePair<string, string>("extracted_files_osint", "true"),
                 new KeyValuePair<string, string>("visualization", "true"),
                 new KeyValuePair<string, string>("file_download", "true"),
                 new KeyValuePair<string, string>("resolve_domains", "true"),
                 new KeyValuePair<string, string>("input_file_yara", "true"),
                 new KeyValuePair<string, string>("extracted_files_yara", "true"),
                 new KeyValuePair<string, string>("whois", "true"),
                 new KeyValuePair<string, string>("ips_meta", "true"),
                 new KeyValuePair<string, string>("images_ocr", "true")
            };
            content.Add(new FormUrlEncodedContent(pairs));
            */

            content.Add(new StringContent("false"), "save_preset");
            content.Add(new StringContent(""),"description");
            content.Add(new StringContent(""),"tags");
            content.Add(new StringContent("false"), "propagate_tags");
            content.Add(new StringContent(""), "password");
            content.Add(new StringContent("false"), "is_private");
            content.Add(new StringContent("false"), "skip_whitelisted");
            content.Add(new StringContent("false"), "rapid_mode");
            content.Add(new StringContent("true"), "osint");
            content.Add(new StringContent("true"), "extended_osint");
            content.Add(new StringContent("true"), "extracted_files_osint");
            content.Add(new StringContent("true"), "visualization");
            content.Add(new StringContent("true"), "files_download");
            content.Add(new StringContent("true"), "resolve_domains");
            content.Add(new StringContent("true"), "input_file_yara");
            content.Add(new StringContent("true"), "extracted_files_yara");
            content.Add(new StringContent("true"), "whois");
            content.Add(new StringContent("true"), "ips_meta");
            content.Add(new StringContent("true"), "images_ocr");

            
            httpRequestMessage.Content = content;
            var response = await client.SendAsync(httpRequestMessage);

            return response;
            
        }
        
        
    }
}
