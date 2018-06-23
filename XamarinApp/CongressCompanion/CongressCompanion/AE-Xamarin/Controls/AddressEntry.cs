using Xamarin.Forms;

namespace AE_Xamarin.Controls
{
    public class AddressEntry : ValidationEntry
    {
        //Store Origional Color
        private Color? TextBaseColor = null;

        public AddressEntry() : base(BasicRegex.RegexPatters.StreetAddressValidation)
        {
            //Set Default Info
            Keyboard = Keyboard.Plain;

            //Add Event To Validate The Number
            Unfocused += AddressEntry_Unfocused;
        }
        private void AddressEntry_Unfocused(object sender, FocusEventArgs e)
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
