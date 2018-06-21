using AE_Xamarin.Forms;
using CongressCompanion.ClassObjects;
using Flex.Controls;
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
    public partial class RepProfilePage : ContentPage
    {
        private Representative CurrentRep;
        public RepProfilePage(Representative local)
        {
            InitializeComponent();
            UpdateThemeColors();
            CurrentRep = local;

            //Load Data
            NameLBL.Text = CurrentRep.FullName;
            TitleLBL.Text = CurrentRep.OfficeName;
            PartyLBL.Text = CurrentRep.Party.ToString();

            //Check For Profile Image
            if (CurrentRep.ImageUrl != null)
            {
                ProfilePhotoIMG.Source = CurrentRep.ImageUrl;
            }
            else
            {
                //Disable The Item
                ImageLayout.IsVisible = false;
                ProfilePhotoIMG.IsVisible = false;
                TitleLayout.Children.Add(NameLBL);
                TitleLayout.Children.Add(TitleLBL);
            }

            //Check For Phone Number
            if (!string.IsNullOrEmpty(CurrentRep.PhoneNumber))
            {
                PhoneLBL.Text = CurrentRep.PhoneNumber;
            }
            else
            {
                //Disable The Item
                PhoneLBL.IsVisible = false;
                PhoneHeader.IsVisible = false;
                PhoneCallBTN.IsEnabled = false;
            }

            //Check For Website
            if (CurrentRep.Website != null)
            {
                WebsiteLBL.Text = CurrentRep.Website.ToString();
            }
            else
            {
                //Disable The Item
                WebsiteBTN.IsEnabled = false;
                WebsiteLBL.IsVisible = false;
                WebsiteHeader.IsVisible = false;
            }

            //Check For Email
            if (string.IsNullOrEmpty(CurrentRep.PrimaryEmail))
            {
                //Disable The Item
                EmailBTN.IsEnabled = false;
            }
        }

        private void UpdateThemeColors()
        {
            //Set Colors
            NameLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            TitleLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            PartyLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            PhoneLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            WebsiteLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;

            PartyHeader.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            PhoneHeader.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            WebsiteHeader.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            CenterLayout.BackgroundColor = AppThemeManager.Instance.CurrentTheme.BackgroundColor;
        }

        private void Icon_Clicked(object sender, EventArgs e)
        {
            //Get Icon Name
            FlexButton ButtonView = (FlexButton)sender;
            string IconName = ButtonView.Icon.ToString();
            IconName = IconName.Replace("File: ", "").Replace(".png", "");

            switch (IconName.ToLower())
            {
                case "phonecall":
                    Device.OpenUri(new Uri($"tel:{CurrentRep.PhoneNumber.ToString()}"));
                    break;
                case "website":
                    Device.OpenUri(CurrentRep.Website);
                    break;
                case "email":
                    Device.OpenUri(new Uri($"mailto:{CurrentRep.PrimaryEmail}"));
                    break;
            }
        }
    }
}