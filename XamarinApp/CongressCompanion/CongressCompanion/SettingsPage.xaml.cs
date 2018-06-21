using AE_Xamarin.Forms;
using CongressCompanion.ClassObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CongressCompanion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();

            //Load The Colors
            UpdateThemeColors();
        }

        private async void EditLocationBTN_Clicked(object sender, EventArgs e)
        {
            //Load The Location Page
            await Navigation.PushAsync(new LocationPage(true));
        }
        private void SaveLocationSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            //Set The Visibility Of The Location Bar
            BoarderLayout.IsVisible = e.Value;
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            //Update The Text Based On Location Type
            if (AppManager.Instance.IsZipcode)
            {
                LocationEntry.Text = $"Zipcode: {AppManager.Instance.UserLocationInfo}";
            }
            else
            {
                LocationEntry.Text = AppManager.Instance.UserLocationInfo;
            }
        }
        public void UpdateThemeColors()
        {
            LocationEntry.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            SaveLocationLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            EditLocationBTN.BackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor;
            SavedLocationLayout.BackgroundColor = AppThemeManager.Instance.CurrentTheme.BackgroundColor;

            //Add More Margin Space For UWP
            if (Device.RuntimePlatform == Device.UWP)
            {
                BoarderLayout.Margin = new Thickness(45, 0);
            }
        }
        protected override bool OnBackButtonPressed()
        {
            //Stop From Going Back To Start Page
            return true;
        }
    }
}