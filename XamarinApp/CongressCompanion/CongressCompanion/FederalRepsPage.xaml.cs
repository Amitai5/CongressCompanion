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
    public partial class FederalRepsPage : ContentPage
    {
        public FederalRepsPage()
        {
            InitializeComponent();

            //Set Font Colors
            FederalLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;

            //Check If Loaded In...
            if (AppManager.Instance.FederalReps != null && AppManager.Instance.FederalReps.Count > 0)
            {
                foreach (Representative FederalRep in AppManager.Instance.FederalReps)
                {
                    StackLayout RepLayout = new StackLayout()
                    {
                        Padding = new Thickness(20, 20, 20, 20),
                        BackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor,
                        GestureRecognizers = { new TapGestureRecognizer { Command = new Command(() => LoadProfilePage(FederalRep)) } }
                    };

                    Label RepresentativeName = new Label()
                    {
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, false),
                        TextColor = AppThemeManager.Instance.CurrentTheme.TextColor,
                        HorizontalTextAlignment = TextAlignment.Start,
                        Text = FederalRep.FullName,
                        FontSize = 24
                    };

                    Label RepresentativeTitle = new Label()
                    {
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, false),
                        TextColor = AppThemeManager.Instance.CurrentTheme.TextColor,
                        HorizontalTextAlignment = TextAlignment.Start,
                        Text = FederalRep.OfficeName,
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
                    HorizontalTextAlignment = TextAlignment.Start,
                    Text = "No Federal Officals Were Found...",
                    FontSize = 18
                };
                MainLayout.Children.Add(NoRepsFound);
            }
        }
        protected override bool OnBackButtonPressed()
        {
            //Stop From Going Back To Start Page
            return true;
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