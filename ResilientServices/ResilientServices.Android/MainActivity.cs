
using System.Net.Http;
using Akavache;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Splat;
using Xamarin.Android.Net;
using Xamarin.Forms;

namespace ResilientServices.Droid
{
    [Activity(Label = "ResilientServices", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            Forms.SetFlags("FastRenderers_Experimental");
            global::Xamarin.Forms.Forms.Init(this, bundle);
            Locator.CurrentMutable.RegisterLazySingleton(() => new AndroidClientHandler(), typeof(HttpMessageHandler));
            LoadApplication(new App());
        }

        protected override void OnDestroy()
        {
            BlobCache.Shutdown().Wait();
            base.OnDestroy();
        }
    }
}

