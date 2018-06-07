using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AE_Xamarin.Forms.Controls
{
    public class RoundBTN : Button
    {
        /// <summary>
        /// The Radius Length Of The Button.
        /// </summary>
        public int ButtonRadius
        {
            set
            {
                //Save Value
                BorderRadius = value;

                //Make The Button Round
                WidthRequest = BorderRadius * 2;
                HeightRequest = BorderRadius * 2;
            }
        }
        
        public RoundBTN()
        {
            //Set Default Color
            BackgroundColor = AppThemeManager.Instance.CurrentTheme.NavBarColor;

            //Set Default Padding
            Margin = new Thickness(5, 5, 5, 5);
        }
    }
}