using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using CongressCompanion.ClassObjects;

namespace CongressCompanion.Droid
{
    [Activity(Label = "Congress Companion", Icon = "@drawable/CongressIcon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            AppManager.Instance.LoadDataFromSave();
            base.OnResume();
        }

        protected override void OnPause()
        {
            AppManager.Instance.SaveUserData();
            base.OnPause();
        }
    }
}

