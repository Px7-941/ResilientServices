﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Fusillade;
using ResilientServices.Dtos;
using ResilientServices.Services;
using Xamarin.Forms;

namespace ResilientServices.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        private readonly IPhotosService _photosService;

        public ICommand GetDataCommand { get; set; }

        public MainViewModel(IPhotosService conferencesService)
        {
            _photosService = conferencesService;
            GetDataCommand = new Command(async () => await GetPhotos());
        }

        public IList<PhotoDto> Photos { get; set; }
        public bool IsLoading { get; set; }

        public async Task GetPhotos()
        {
            IsLoading = true;

            var photos = await _photosService
                                            .GetPhotos(Priority.Background)
                                            .ConfigureAwait(false);
            CachePhotos(photos);

            IsLoading = false;

            Photos = photos;
        }

        private void CachePhotos(IList<PhotoDto> photos)
        {
            foreach (var id in photos.Take(10).Select(x => x.Id))
            {
                _photosService.GetPhoto(Priority.Speculative, id);
            }
        }
    }
}
