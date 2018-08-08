using System;
using System.Collections.Generic;
using System.Net;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Akavache;
using Fusillade;
using Plugin.Connectivity;
using Polly;
using ResilientServices.Dtos;

namespace ResilientServices.Services
{
    public class PhotosService : IPhotosService
    {
        private readonly IApiService _apiService;

        public PhotosService(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<List<PhotoDto>> GetPhotos(Priority priority)
        {
            var cache = BlobCache.LocalMachine;
            var cachedPhotos = cache.GetAndFetchLatest("photos", () => GetRemotePhotosAsync(priority),
                offset =>
                {
                    TimeSpan elapsed = DateTimeOffset.Now - offset;
                    return elapsed > new TimeSpan(hours: 1, minutes: 0, seconds: 0);
                });

            var photos = await cachedPhotos.FirstOrDefaultAsync();
            return photos;
        }

        public async Task<PhotoDto> GetPhoto(Priority priority, int id)
        {
            var cachedPhoto = BlobCache.LocalMachine.GetAndFetchLatest(id.ToString(), () => GetRemotePhoto(priority, id), offset =>
            {
                TimeSpan elapsed = DateTimeOffset.Now - offset;
                return elapsed > new TimeSpan(hours: 0, minutes: 30, seconds: 0);
            });

            var photo = await cachedPhoto.FirstOrDefaultAsync();

            return photo;
        }


        private async Task<List<PhotoDto>> GetRemotePhotosAsync(Priority priority)
        {
            List<PhotoDto> photos = null;
            Task<List<PhotoDto>> getPhotosTask;
            switch (priority)
            {
                case Priority.Background:
                    getPhotosTask = _apiService.Background.GetPhotos();
                    break;
                case Priority.UserInitiated:
                    getPhotosTask = _apiService.UserInitiated.GetPhotos();
                    break;
                case Priority.Speculative:
                    getPhotosTask = _apiService.Speculative.GetPhotos();
                    break;
                default:
                    getPhotosTask = _apiService.UserInitiated.GetPhotos();
                    break;
            }
            
            if (CrossConnectivity.Current.IsConnected)
            {
                photos = await Policy
                      .Handle<WebException>()
                      .WaitAndRetryAsync
                      (
                        retryCount: 5,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                      )
                      .ExecuteAsync(() => getPhotosTask);
            }
            return photos;
        }

        public async Task<PhotoDto> GetRemotePhoto(Priority priority, int id)
        {
            PhotoDto conference = null;

            Task<PhotoDto> getPhotoTask;
            switch (priority)
            {
                case Priority.Background:
                    getPhotoTask = _apiService.Background.GetPhoto(id);
                    break;
                case Priority.UserInitiated:
                    getPhotoTask = _apiService.UserInitiated.GetPhoto(id);
                    break;
                case Priority.Speculative:
                    getPhotoTask = _apiService.Speculative.GetPhoto(id);
                    break;
                default:
                    getPhotoTask = _apiService.UserInitiated.GetPhoto(id);
                    break;
            }

            if (CrossConnectivity.Current.IsConnected)
            {
                conference = await Policy
                    .Handle<Exception>()
                    .RetryAsync(retryCount: 5)
                    .ExecuteAsync(async () => await getPhotoTask);
            }

            return conference;
        }

    }
}