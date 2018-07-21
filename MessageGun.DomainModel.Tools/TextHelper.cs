using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MessageGun.DomainModel.Tools
{
    public static class TextHelper
    {

        private static string  regNumbers = "[0-9]";

        public static string PreparePhoneNumber(string phoneNumber)
        {
            StringBuilder correctPhoneNumber = new StringBuilder();

            foreach (Match m in Regex.Matches(phoneNumber, regNumbers))
            {
                correctPhoneNumber.Append(m.Value);
            }

            return correctPhoneNumber.ToString();
        }


        /// <summary>
        /// True if phone format is 79999999999
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool CheckPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.Length == 11)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static string PrepareAndCheckPhone(string phoneNumber)
        {
            string phone = PreparePhoneNumber(phoneNumber);

            if (CheckPhoneNumber(phone))
            {
                return phone;
            }
            else
            {
                throw new Exception($"Not valid phone number: {phoneNumber}");
            }
        }
    }
}
