using AE_Xamarin.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AE_Xamarin.Forms.Controls
{
    public class ValidationEntry : Entry
    {
        /// <summary>
        /// The Validity Of The Text.
        /// </summary>
        public bool IsValid
        {
            get
            {
                //Check Regex
                return Regex.IsMatch(Text, RegexPattern);
            }
        }
        private readonly string RegexPattern = "";

        /// <summary>
        /// Fired When The Entry Has Finished Validating The Email.
        /// </summary>
        public new event EventHandler<ValidationEventArgs> FinishedValidation;
        public static readonly BindableProperty FinishedValidationProp = BindableProperty.Create("Finished Validation", typeof(ValidationEventArgs), typeof(EventHandler<ValidationEventArgs>));

        public ValidationEntry(string Pattern)
        {
            //Save Pattern
            RegexPattern = Pattern;

            //Add Event To Validate The Number
            Unfocused += ValidationEntry_Unfocused;
        }
        private void ValidationEntry_Unfocused(object sender, FocusEventArgs e)
        {
            //Fire Event
            FinishedValidation?.Invoke(this, new ValidationEventArgs(Text, IsValid));
        }
    }
}
