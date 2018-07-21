using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace MessageGun.DomainModel.Tools.Converters
{
    public class RegularParser
    {

        private const string _regularConfig = @"Converters\RegularConfig.json";

        private IDictionary<string, string> regularDictionary;

        public RegularParser()
        {
            CheckConfig();

            regularDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(File.ReadAllText(_regularConfig));
        }

        public IDictionary<string, string> Parse(string text)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> keyValue in regularDictionary)
            {
                string necessaryVaklue = Regex.Match(text, keyValue.Value).ToString();
                dictionary.Add(keyValue.Key, necessaryVaklue);
            }
            return dictionary;
        }

        private void CheckConfig()
        {
            if (File.Exists(_regularConfig))
            {
                string text = File.ReadAllText(_regularConfig);
                if (!IsJson(text))
                {
                    throw new IOException("файл RegularConfig.json поврежден");
                }
            }
            else
            {
                throw new IOException("файл RegularConfig.json отсутствует");
            }

        }


        private bool IsJson(string input)
        {
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                   || input.StartsWith("[") && input.EndsWith("]");
        }

    }
}
