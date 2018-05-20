using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AE_Xamarin.Misc
{
    public static class BasicRegex
    {
        /// <summary>
        /// The Regex Patterns Used By The Class.
        /// </summary>
        public class RegexPatters
        {
            /// <summary>
            /// The Validation REGEX String For Names.
            /// </summary>
            public const string NameValidation = @"(?=\w{8,})(^([A-Z|[a-z]|\d)*(?=[A-Z]+)([A-Z|[a-z]|\d)*$)";

            /// <summary>
            /// The Validation REGEX String For Emails.
            /// </summary>
            public const string EmailValidation = @"^([A-Z|[a-z]|\d|\.){1,64}\@(?!.{256})((?<=\.|\@)([A-Z|[a-z]|\d){1,63}(?(?=\.)\.|([A-Z|[a-z]|\d){1,63}))+$";

            /// <summary>
            /// The Validation REGEX String For Phone Numbers.
            /// </summary>
            public const string PhoneValidation = @"^(\+\d)?\(?(\d{3})(-|\))(?<=\(\d{3}\)|(?<!\()\d{3}-)-?\d{3}-\d{4}$";

            /// <summary>
            /// The Validation REGEX String For Zipcodes.
            /// </summary>
            public const string ZipcodeValidation = @"^\d{5}(?:[-\s]\d{4})?$";

            /// <summary>
            /// The Validation REGEX String For Street Addresses.
            /// </summary>
            public const string StreetAddressValidation = @"^[A-Za-z0-9]+(?:\s[A-Za-z0-9'_-]+)+$";
        }

        /// <summary>
        /// Checks If The Given Name Is Valid.
        /// </summary>
        /// <param name="NamePortion">The Name To Check.</param>
        /// <returns>Bool Valid.</returns>
        public static bool IsValidName(string NamePortion)
        {
            //Make Sure There Is Text There
            if (String.IsNullOrEmpty(NamePortion))
            {
                return false;
            }

            //Run The Regex
            return Regex.IsMatch(NamePortion, RegexPatters.NameValidation);
        }

        /// <summary>
        /// Checks If The Given Phone Number Is Valid.
        /// </summary>
        /// <param name="Number">The Phone Number To Check.</param>
        /// <returns>Bool Valid.</returns>
        public static bool IsValidPhoneNum(string Number)
        {
            //Make Sure There Is Text There
            if (String.IsNullOrEmpty(Number))
            {
                return false;
            }

            //Run The Regex
            return Regex.IsMatch(Number, RegexPatters.PhoneValidation);
        }

        /// <summary>
        /// Checks If The Given Email Is Valid.
        /// </summary>
        /// <param name="Email">The Email To Check.</param>
        /// <returns>Bool Valid.</returns>
        public static bool IsValidEmail(string Email)
        {
            //Make Sure There Is Text There
            if (String.IsNullOrEmpty(Email))
            {
                return false;
            }

            //Run The Regex
            return Regex.IsMatch(Email, RegexPatters.EmailValidation);
        }

        /// <summary>
        /// Checks If The Given Zipcode Is Valid.
        /// </summary>
        /// <param name="Address">The Zipcode To Check.</param>
        /// <returns>Bool Valid.</returns>
        public static bool IsValidZipcode(string Zipcode)
        {
            //Make Sure There Is Text There
            if (String.IsNullOrEmpty(Zipcode))
            {
                return false;
            }

            //Run The Regex
            return Regex.IsMatch(Zipcode, RegexPatters.ZipcodeValidation);
        }

        /// <summary>
        /// Checks If The Given Street Address Is Valid.
        /// </summary>
        /// <param name="Address">The Address To Check.</param>
        /// <returns>Bool Valid.</returns>
        public static bool IsValidStreetAddress(string Address)
        {
            //Make Sure There Is Text There
            if (String.IsNullOrEmpty(Address))
            {
                return false;
            }

            //Run The Regex
            return Regex.IsMatch(Address, RegexPatters.StreetAddressValidation);
        }
    }
}
