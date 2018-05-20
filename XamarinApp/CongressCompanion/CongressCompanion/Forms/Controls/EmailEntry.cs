using AE_Xamarin.Misc;
using Xamarin.Forms;

namespace AE_Xamarin.Forms.Controls
{
    public class EmailEntry : ValidationEntry
    {
        //Store Origional Color
        private Color? TextBaseColor = null;

        public EmailEntry() : base(BasicRegex.RegexPatters.EmailValidation)
        {
            //Set Default Info
            Keyboard = Keyboard.Email;
            Placeholder = "example@mail.com";

            //Add Event To Validate The Number
            Unfocused += EmailEntry_Unfocused;
        }
        private void EmailEntry_Unfocused(object sender, FocusEventArgs e)
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
