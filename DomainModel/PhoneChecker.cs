using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageGun.DomainModel
{
    public class PhoneChecker
    {
        private  bool _doCheck = false;

        public bool DoCheck
        {
            get => _doCheck;
            set => _doCheck = value;
        }

        private IDictionary<int, string> dictionaryPhones;

        public PhoneChecker(IDictionary<int,string> phonesList)
        {
            dictionaryPhones = phonesList;
        }

        public int IsAvaliable(string phone)
        {
            foreach (KeyValuePair<int,string> kpValue in dictionaryPhones)
            {


                string cutPhone = kpValue.Value.Trim();
                if (cutPhone == phone)
                {
                    return kpValue.Key;
                }
            }

            return -1;
        }

    }
}
