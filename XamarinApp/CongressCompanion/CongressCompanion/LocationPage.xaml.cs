using AE_Xamarin.Forms;
using CongressCompanion.ClassObjects;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Plugin.Geolocator;

namespace CongressCompanion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationPage : ContentPage
    {
        bool IsLoadingNextPage = false;
        public LocationPage()
        {
            InitializeComponent();

            AppThemeManager.Instance.AppThemeChange += Instance_AppThemeChange;
            CongressSealImg.Source = ImageSource.FromFile("CongressIcon.png");
            LoadingIcon.Color = Color.FromHex("#ECAB66"); //Set The Loading Icon To A Goldish
            ReloadThemeColors();
        }

        public async void LoadNextPage(object sender, EventArgs e)
        {
            //Check For Nothing
            if (string.IsNullOrEmpty(AddressTxtBox.Text))
            {
                return;
            }

            //Check Loading
            if (IsLoadingNextPage)
            {
                return;
            }
            IsLoadingNextPage = true;

            //Show The Icon
            LoadingIcon.IsVisible = true;
            LoadingIcon.IsRunning = true;

            //Check Valididty
            if (!AddressTxtBox.IsValid)
            {
                await DisplayAlert("Not Valid", "Not A Valid Address", "OK");
                LoadingIcon.IsVisible = false;
                LoadingIcon.IsRunning = false;
                IsLoadingNextPage = false;
                return;
            }

            //Load All The Info
            AppManager.Instance.UserAddress = AddressTxtBox.Text;
            await AppManager.Instance.LoadRepresentatives();
            LoadingIcon.IsVisible = false;
            LoadingIcon.IsRunning = false;
            IsLoadingNextPage = false;

            //Load Next Page
            await Navigation.PushModalAsync(new MainPage());
        }

        private void ReloadThemeColors()
        {
            //Set Background
            BackgroundColor = AppThemeManager.Instance.CurrentTheme.BackgroundColor;
            ConfirmBTN.BackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor;

            //Set Text-Colors
            ConfirmBTN.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            AddressTxtBox.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            AddressTxtBox.PlaceholderColor = AppThemeManager.Instance.CurrentTheme.TextColor;
        }
        private void Instance_AppThemeChange(object sender, ThemeChangeArgs e)
        {
            ReloadThemeColors();
        }
    }
}