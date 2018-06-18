using AE_Xamarin.Forms;
using CongressCompanion.ClassObjects;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CongressCompanion
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LocationPage : ContentPage
    {
        bool IsLoadingNextPage = false;
        public LocationPage()
        {
            InitializeComponent();

            //Check If The Location Is Already Loaded In
            if (!string.IsNullOrEmpty(AppManager.Instance.UserLocationInfo))
            {
                Navigation.PushModalAsync(new MainPage());
                Navigation.RemovePage(this);
                return;
            }

            //Otherwise Load This
            AppThemeManager.Instance.AppThemeChange += Instance_AppThemeChange;
            CongressSealImg.Source = ImageSource.FromFile("CongressIcon.png");
            LoadingIcon.Color = Color.FromHex("#ECAB66"); //Set The Loading Icon To A Goldish
            ReloadThemeColors();

            //Set State Values (Add State Temporarily)
            string[] StateAbrev = new string[] { "State", "AK", "AL", "AR", "AS", "AZ", "CA", "CO", "CT", "DC", "DE", "FL", "GA", "GU", "HI", "IA", "ID", "IL", "IN", "KS", "KY", "LA",
                "MA", "MD", "ME", "MI", "MN", "MO", "MS", "MT", "NC", "ND", "NE", "NH", "NJ", "NM", "NV", "NY", "OH", "OK", "OR", "PA", "PR", "RI", "SC", "SD", "TN", "TX", "UT",
                "VA", "VI", "VT", "WA", "WI", "WV", "WY" };
            for (int i = 0; i < StateAbrev.Length; i++)
            {
                StatePicker.Items.Add(StateAbrev[i]);
            }
            StatePicker.SelectedIndex = 0;
        }

        public async void LoadNextPage(object sender, EventArgs e)
        {
            //Check Loading
            if (IsLoadingNextPage)
            {
                return;
            }
            IsLoadingNextPage = true;
            bool IsZipcodeSearch = false;
            LoadingIcon.IsVisible = true;

            //Check If Any Of Them Are Null Or Empty
            if (string.IsNullOrEmpty(AddressTxtBox.Text) && string.IsNullOrEmpty(CityTxtBox.Text)
                && string.IsNullOrEmpty(ZipcodeTxtBox.Text))
            {
                LoadingIcon.IsVisible = false;
                IsLoadingNextPage = false;
                return;
            }
            else
            {
                //Set Zipcode Search
                IsZipcodeSearch = !string.IsNullOrEmpty(ZipcodeTxtBox.Text);
            }

            //Check How They Want To Search
            if (IsZipcodeSearch)
            {
                AppManager.Instance.UserLocationInfo = ZipcodeTxtBox.Text.Trim();

                //Check Zipcode Validity
                if (!ZipcodeTxtBox.IsValid)
                {
                    await DisplayAlert("Not Valid", "Not A Valid Zipcode", "OK");
                    LoadingIcon.IsVisible = false;
                    IsLoadingNextPage = false;
                    return;
                }
            }
            else
            {
                //Check Address Validity
                if (!AddressTxtBox.IsValid)
                {
                    await DisplayAlert("Not Valid", "Not A Valid Address", "OK");
                    LoadingIcon.IsVisible = false;
                    IsLoadingNextPage = false;
                    return;
                }
                else if (!CityTxtBox.IsValid)
                {
                    await DisplayAlert("Not Valid", "Not A Valid City", "OK");
                    LoadingIcon.IsVisible = false;
                    IsLoadingNextPage = false;
                    return;
                }

                //Get Address Info
                string Address = $"{AddressTxtBox.Text.Trim()}, {CityTxtBox.Text.Trim()} {StatePicker.SelectedItem.ToString()}";
                AppManager.Instance.UserLocationInfo = Address.Trim();
            }

            //Load All The Info
            bool Success = await AppManager.Instance.LoadRepresentatives();
            if (!Success)
            {
                await DisplayAlert("Not Valid", "Not A Valid Address Or Zipcode", "OK");
                LoadingIcon.IsVisible = false;
                IsLoadingNextPage = false;
                return;
            }

            //Load Next Page
            IsLoadingNextPage = false;
            LoadingIcon.IsVisible = false;
            await Navigation.PushModalAsync(new MainPage());
        }

        private void ReloadThemeColors()
        {
            //Set Background
            BackgroundColor = AppThemeManager.Instance.CurrentTheme.BackgroundColor;
            ConfirmBTN.BackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor;

            //Set Text-Colors
            ConfirmBTN.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            StatePicker.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            AddressTxtBox.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            AddressTxtBox.PlaceholderColor = AppThemeManager.Instance.CurrentTheme.TextColor;
        }
        private void Instance_AppThemeChange(object sender, ThemeChangeArgs e)
        {
            ReloadThemeColors();
        }

        bool JustRemoved = false;
        private void StatePicker_Focused(object sender, FocusEventArgs e)
        {
            //Remove The State The First Time It Loads
            if (StatePicker.Items.Contains("State"))
            {
                JustRemoved = true;
                StatePicker.Items.RemoveAt(0);
            }
        }
        private void StatePicker_Unfocused(object sender, FocusEventArgs e)
        {
            //Run The Offset Of The State Picker Index
            if (JustRemoved)
            {
                StatePicker.SelectedIndex--;
                JustRemoved = false;
            }
        }

        private void AddressTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check If They Are Using This Side
            if (!string.IsNullOrEmpty(AddressTxtBox.Text))
            {
                AddressFieldsHolder.IsVisible = true;
                ZipcodeTxtBox.IsVisible = false;
                SeparatorLBL.IsVisible = false;
            }
            else
            {
                AddressFieldsHolder.IsVisible = false;
                ZipcodeTxtBox.IsVisible = true;
                SeparatorLBL.IsVisible = true;
            }
        }
        private void ZipcodeTxtBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check If They Are Using This Side
            if (!string.IsNullOrEmpty(ZipcodeTxtBox.Text))
            {
                AddressTxtBox.IsVisible = false;
                SeparatorLBL.IsVisible = false;
            }
            else
            {
                AddressTxtBox.IsVisible = true;
                SeparatorLBL.IsVisible = true;
            }
        }
    }
}