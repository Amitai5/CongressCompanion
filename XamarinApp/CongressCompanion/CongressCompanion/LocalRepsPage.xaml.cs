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
    public partial class LocalRepsPage : ContentPage
    {
        public LocalRepsPage()
        {
            InitializeComponent();

            //Set Font Colors
            LocalLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;

            //Check If Loaded In...
            if (AppManager.Instance.LocalReps != null && AppManager.Instance.LocalReps.Count > 0)
            {
                foreach (Representative LocalRep in AppManager.Instance.LocalReps)
                {
                    StackLayout RepLayout = new StackLayout()
                    {
                        Padding = new Thickness(20, 20, 20, 20),
                        BackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor,
                        GestureRecognizers = { new TapGestureRecognizer { Command = new Command(() => LoadProfilePage(LocalRep)) } }
                    };

                    Label RepresentativeName = new Label()
                    {
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, false),
                        TextColor = AppThemeManager.Instance.CurrentTheme.TextColor,
                        HorizontalTextAlignment = TextAlignment.Start,
                        Text = LocalRep.FullName, //Add State Prefix
                        FontSize = 24
                    };

                    Label RepresentativeTitle = new Label()
                    {
                        HorizontalOptions = new LayoutOptions(LayoutAlignment.Start, false),
                        TextColor = AppThemeManager.Instance.CurrentTheme.TextColor,
                        HorizontalTextAlignment = TextAlignment.Start,
                        Text = LocalRep.OfficeName,
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
                    Text = "No Local Officals Were Found...",
                    FontSize = 18
                };
                MainLayout.Children.Add(NoRepsFound);
            }
        }

        private void LoadProfilePage(Representative LocalRep)
        {
            //Create New Page
            ContentPage ProfilePage = new RepProfilePage(LocalRep)
            {
                Title = "Representative Profile"
            };

            //Push The Page
            Navigation.PushAsync(ProfilePage);
        }
    }
}