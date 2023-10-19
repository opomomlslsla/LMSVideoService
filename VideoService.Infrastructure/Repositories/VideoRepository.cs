using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using Microsoft.Extensions.Options;
using VideoService.Domain.Models;
using VideoService.Domain.DTO;
using Amazon.Runtime.SharedInterfaces;

namespace VideoService.Infrastructure.Repositories
{
    public class VideoRepository
    {
        private readonly IMongoCollection<VideoModel> _VideosCollection;
        private readonly IGridFSBucket _GridFSBucket;


        public VideoRepository(
            IOptions<DataBaseSettings> DatabaseSettings)
        {
            var mongoClient = new MongoClient(DatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(DatabaseSettings.Value.DatabaseName);

            _VideosCollection = mongoDatabase.GetCollection<VideoModel>(DatabaseSettings.Value.VideoCollectionName);

            _GridFSBucket = new GridFSBucket(mongoDatabase);
        }


        
        public async Task<Stream> GetVideoById(string id)
        {
            var result = await _VideosCollection.Find(i => i.Id == id).FirstOrDefaultAsync();

            Stream videoStream = await _GridFSBucket.OpenDownloadStreamAsync(new ObjectId(id));

            return videoStream;
        }
        


        public async Task<VideoModel> GetVideoModelById(string id)
        {
            ObjectId _id = new ObjectId(id);
            var result = await _VideosCollection.Find(i => i.Id == id).FirstOrDefaultAsync();
            return result;
        }

        public async Task<VideoModel> AddVideoToMongo(VideoModelDTO videoModelDTO)
        {
            var fileStream = videoModelDTO.formfile.OpenReadStream();
            string id = (await _GridFSBucket.UploadFromStreamAsync(videoModelDTO.formfile.FileName, fileStream)).ToString();
            
            VideoModel videoModel = new VideoModel
            {
                Name = videoModelDTO.formfile.FileName,
                Id = id,
                IsConnectedToDocument = videoModelDTO.IsConnectedToDocument
            };

            videoModel.SetDocementId(videoModelDTO.DocumentID);

            await _VideosCollection.InsertOneAsync(videoModel);
            return videoModel;
        }

        /*
        public Task UpdateF(VideoModel video)
        {
            var filter = Builders<VideoModel>.Filter.Eq("Id", video.Id);
            return _VideosCollection.ReplaceOneAsync(filter, video, new ReplaceOptions { IsUpsert = true });
        }
        */


        public async void DeleteVideo(string id)
        {
            await _VideosCollection.DeleteOneAsync(i => i.Id == id);
            await _GridFSBucket.DeleteAsync(new ObjectId(id));
        }
    }

}

