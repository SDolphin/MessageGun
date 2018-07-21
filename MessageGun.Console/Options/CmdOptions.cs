using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CommandLine;
using CommandLine.Text;


namespace MessageGun.Console.Options
{
    public enum MqOptionEnum
    {
        OnlyRead,
        Count,
        GetAllAndClean,
        Error
    }

    public class CmdOptions
    {

        


        [Option('n', "new-connection", Required = false, HelpText = "if true, you will start two-factor autorize")]
        public string NewConnectionInfo { get; set; }

        [Option('m', "mq-options", Required = false, HelpText = "read / count / getall")]
        public string MqOption { get; set; }


        [Option('h', "help", Required = false, HelpText = "Help info")]
        public string HelpInfo
        {
            get
            {
                return "-h / --help help info\n";
            }
        }

        public static bool ConnectionStringCheck(string s)
        {
            if (s.ToLower() == "true" || s.ToLower() == "false")
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public static MqOptionEnum MqOptionStringCheck(string s)
        {
            MqOptionEnum mqOptionEnum = MqOptionEnum.Error;

            if (s == "read")
            {
                mqOptionEnum = MqOptionEnum.OnlyRead;
            }
            else if (s == "count")
            {
                mqOptionEnum = MqOptionEnum.Count;
            }
            else if (s == "getall")
            {
                mqOptionEnum = MqOptionEnum.GetAllAndClean;
            }

            return mqOptionEnum;
        }


    }
    
}
