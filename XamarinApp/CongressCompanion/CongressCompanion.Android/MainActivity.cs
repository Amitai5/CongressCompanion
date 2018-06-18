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
            LoadData();
        }

        protected override void OnDestroy()
        {
            SaveData();
            base.OnDestroy();
        }

        // Function called from OnCreate
        protected void LoadData()
        {
            //Retrieve 
            ISharedPreferences Prefs = Application.Context.GetSharedPreferences("CongressCompanion", FileCreationMode.Private);
            AppManager.Instance.UserLocationInfo = Prefs.GetString("UserLocationInfo", null);
        }

        // Function called from OnDestroy
        protected void SaveData()
        {
            //Store
            ISharedPreferences Prefs = Application.Context.GetSharedPreferences("CongressCompanion", FileCreationMode.Private);
            var PrefsEditor = Prefs.Edit();
            PrefsEditor.PutString("UserLocationInfo", AppManager.Instance.UserLocationInfo);
            PrefsEditor.Commit();
        }
    }
}

