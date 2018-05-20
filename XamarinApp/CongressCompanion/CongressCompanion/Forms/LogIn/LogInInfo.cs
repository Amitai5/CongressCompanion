using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace AE_Xamarin.Forms.LogIn
{
    public class LogInInfo
    {
        public string Username { get; private set; }
        public string Password { get; private set; }

        /// <summary>
        /// The Log In Info To Send The API.
        /// </summary>
        /// <param name="User">The User's Username.</param>
        /// <param name="Pass">The User's Password</param>
        /// <param name="Salt">The User's Salt.</param>
        public LogInInfo(string User, string Pass, string Salt, SaltingMethod Method)
        {
            //Save Username
            Username = User;

            //Start The Peppering Of The Password
            string SaltedPass = "";
            for (int i = 0; i < Pass.Length; i++)
            {
                switch (Method)
                {
                    case SaltingMethod.Simple:
                        SaltedPass += Salt[i];
                        SaltedPass += Pass[i < Pass.Length ? i : i + 1];
                        break;
                    case SaltingMethod.EveryOther:
                        SaltedPass += i % 2 ==0 ? Salt[i].ToString() : "";
                        break;
                    case SaltingMethod.FirstAndLast:
                        SaltedPass += Salt[i > 0 ? i - 1 : i];
                        SaltedPass += Salt[i < Salt.Length ? i : i + 1];
                        break;
                }

                //Add Part Of The Pass
                SaltedPass += Pass[i];
            }
            SaltedPass += Salt.Substring(0, Salt.Length / 2);

            //Set The Password
            Password = SaltedPass;
        }
    }
}
