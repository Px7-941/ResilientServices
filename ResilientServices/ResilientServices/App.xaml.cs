using ResilientServices.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace ResilientServices
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Akavache.Sqlite3.Registrations.Start("ApplicationName", () => SQLitePCL.Batteries_V2.Init());
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
