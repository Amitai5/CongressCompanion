using AE_Xamarin.Misc;
using Xamarin.Forms;

namespace AE_Xamarin.Forms.Controls
{
    public class ZipcodeEntry : ValidationEntry
    {
        //Store Origional Color
        private Color? TextBaseColor = null;

        public ZipcodeEntry() : base(BasicRegex.RegexPatters.ZipcodeValidation)
        {
            //Set Default Info
            Keyboard = Keyboard.Telephone;
            Placeholder = "91302";

            //Add Event To Validate The Number
            Unfocused += ZipcodeEntry_Unfocused;
        }
        private void ZipcodeEntry_Unfocused(object sender, FocusEventArgs e)
        {
            //Check Validity
            if (!IsValid)
            {
                //Save The Color
                TextBaseColor = TextColor;
                TextColor = Color.Red;
            }

            //Make Sure We Changed It In The First Place
            else if (TextBaseColor.HasValue)
            {
                TextColor = TextBaseColor.Value;
            }
        }
    }
}