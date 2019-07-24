using System.ComponentModel;
using ResilientServices.Services;
using ResilientServices.ViewModels;
using Xamarin.Forms;

namespace ResilientServices.Pages
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private readonly MainViewModel _viewModel;
        public const string PlaceholderConfApiUrl = "https://jsonplaceholder.typicode.com";

        public MainPage()
        {
            InitializeComponent();

            var apiService = new ApiService(PlaceholderConfApiUrl);
            var service = new PhotosService(apiService);

            _viewModel = new MainViewModel(service);

            BindingContext = _viewModel;
            _viewModel.GetDataCommand.Execute(null);
        }
    }
}
