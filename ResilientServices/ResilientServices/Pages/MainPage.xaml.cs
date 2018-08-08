using ResilientServices.Services;
using ResilientServices.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ResilientServices.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
			
			this.BindingContext = _viewModel;
            _viewModel.GetDataCommand.Execute(null);
        }
    }
}
