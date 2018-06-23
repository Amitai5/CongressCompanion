using AE_Xamarin.Managers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CongressCompanion
{
    public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            AppManager.Instance.ToString(); //Create App Manager

            //Load The Saved Data
            AppManager.Instance.LoadSavedData();

            //Change The Color of The Page Headers
            NavigationPage Nav = new NavigationPage
            {
                BarBackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor,
                BarTextColor = AppThemeManager.Instance.CurrentTheme.TextColor
            };

            //Set The Title Of The First Page
            Nav.PushAsync(new LocationPage() { Title = "Default Location" });
            MainPage = Nav;
        }

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
