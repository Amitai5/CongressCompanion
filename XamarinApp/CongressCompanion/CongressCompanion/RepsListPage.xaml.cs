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
	public partial class RepsListPage : ContentPage
	{
		public RepsListPage (RepLoadType Type)
		{
			InitializeComponent ();

            //Set Color And Text Of Title
            TitleLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            TitleLBL.Text = $"{Type.ToString()} Officials";

            //Load Up The Reps
            LoadReps(Type);
        }
        protected override bool OnBackButtonPressed()
        {
            //Stop From Going Back To Start Page
            return true;
        }

        private async void LoadReps(RepLoadType RepType)
        {
            //Find Type Of Reps
            List<Representative> RepsToLoad = null;
            switch (RepType)
            {
                case RepLoadType.Local:
                    RepsToLoad = AppManager.Instance.LocalReps;
                    break;
                case RepLoadType.State:
                    RepsToLoad = AppManager.Instance.StateReps;
                    break;
                case RepLoadType.Federal:
                    RepsToLoad = AppManager.Instance.FederalReps;
                    break;
            }

            //Check If The Data Has Not Been Loaded
            if(RepsToLoad == null || RepsToLoad.Count == 0)
            {
                //Load In Reps If Saved Location Is On
                if (AppManager.Instance.ShouldSaveLocation &&
                    !string.IsNullOrEmpty(AppManager.Instance.UserLocationInfo))
                {
                    //Load The Data With The Saved Location
                    await AppManager.Instance.LoadRepresentatives();

                    //Retry To Load Data
                    switch (RepType)
                    {
                        case RepLoadType.Local:
                            RepsToLoad = AppManager.Instance.LocalReps;
                            break;
                        case RepLoadType.State:
                            RepsToLoad = AppManager.Instance.StateReps;
                            break;
                        case RepLoadType.Federal:
                            RepsToLoad = AppManager.Instance.FederalReps;
                            break;
                    }
                }
            }

            //Check If Loaded In...
            if (RepsToLoad != null && RepsToLoad.Count > 0)
            {
                foreach (Representative CurrentRep in RepsToLoad)
                {
                    StackLayout RepLayout = new StackLayout()
                    {
                        Padding = new Thickness(20, 20, 20, 20),
                        BackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor,
                        GestureRecognizers = { new TapGestureRecognizer { Command = new Command(() => LoadProfilePage(CurrentRep)) } }
                    };

                    Label RepresentativeName = new Label()
                    {
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, false),
                        TextColor = AppThemeManager.Instance.CurrentTheme.TextColor,
                        HorizontalTextAlignment = TextAlignment.Start,
                        Text = CurrentRep.FullName,
                        FontSize = 24
                    };

                    Label RepresentativeTitle = new Label()
                    {
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, false),
                        TextColor = AppThemeManager.Instance.CurrentTheme.TextColor,
                        HorizontalTextAlignment = TextAlignment.Start,
                        Text = CurrentRep.OfficeName,
                        FontSize = 18
                    };

                    RepLayout.Children.Add(RepresentativeName);
                    RepLayout.Children.Add(RepresentativeTitle);
                    MainLayout.Children.Add(RepLayout);
                }
            }
            else
            {
                //Show No Reps Label
                Label NoRepsFound = new Label()
                {
                    HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                    VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                    TextColor = AppThemeManager.Instance.CurrentTheme.TextColor,
                    HorizontalTextAlignment = TextAlignment.Center,
                    Text = $"Could Not Find {RepType.ToString()} Officals...",
                    FontSize = 18
                };
                MainLayout.Children.Add(NoRepsFound);

                //Check If They Are Not Using Full Address
                if (AppManager.Instance.IsZipcode)
                {
                    Label UseZipcodeLBL = new Label()
                    {
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                        VerticalOptions = new LayoutOptions(LayoutAlignment.Center, false),
                        TextColor = AppThemeManager.Instance.CurrentTheme.TextColor,
                        HorizontalTextAlignment = TextAlignment.Center,
                        Text = "For More Accurate Results, Use The Full Address Instead Of Just A Zipcode.",
                        FontSize = 14
                    };
                    MainLayout.Children.Add(UseZipcodeLBL);
                }
            }
        }
        private void LoadProfilePage(Representative CurrentRep)
        {
            if (CurrentRep.FullName.ToLower() != "vacant")
            {
                //Create New Page
                ContentPage ProfilePage = new RepProfilePage(CurrentRep)
                {
                    Title = "Representative Profile"
                };

                //Push The Page
                Navigation.PushAsync(ProfilePage);
            }
            else
            {
                //Show Errorish Message
                DisplayAlert("Vacant Seat", "The Desired Seat Does Not Yet Have A Representative", "Ok");
            }
        }
    }
}