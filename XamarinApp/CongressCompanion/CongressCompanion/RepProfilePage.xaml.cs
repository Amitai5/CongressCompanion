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
    public partial class RepProfilePage : ContentPage
    {
        private Representative LocalRep;
        public RepProfilePage(Representative local)
        {
            InitializeComponent();
            LocalRep = local;

            //Set Colors
            NameLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            PartyLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            TitleLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            PhoneLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            WebsiteLBL.TextColor = AppThemeManager.Instance.CurrentTheme.TextColor;
            ContactLayout.BackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor;

            //Load Data
            NameLBL.Text = LocalRep.FullName;
            TitleLBL.Text = LocalRep.OfficeName;
            PartyLBL.Text = $"Political Alignment: {LocalRep.Party}";

            //Check For Profile Image
            if (LocalRep.ImageUrl != null)
            {
                ProfilePhotoIMG.Source = LocalRep.ImageUrl;
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
            if (!string.IsNullOrEmpty(LocalRep.PhoneNumber))
            {
                PhoneLBL.Text = LocalRep.PhoneNumber;
            }
            else
            {
                //Disable The Item
                PhoneLBL.IsVisible = false;
                //CallBTN.IsVisible = false;
            }

            //Check For Website
            if (LocalRep.Website != null)
            {
                WebsiteLBL.Text = LocalRep.Website.ToString();
            }
            else
            {
                //Disable The Item
                WebsiteLBL.IsVisible = false;
            }
        }
    }
}