using MessageGun.Console.Options;
using MQ.MDomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MQ.MClientModel.Common;

namespace MessageGun.Console
{
    public static class ConsoleCommands
    {
        public static void MqOption(MqOptionEnum mqopt, MQProperties prop, string queueName)
        {
            using (MDomainModel domainModel = new MDomainModel(prop))
            {
                if (mqopt == MqOptionEnum.Count)
                {
                    int i = 0;

                    System.Console.Write($"In Main Queue : {i} messages");
                    System.Console.CursorLeft = 0;

                    foreach (string s in domainModel.ReadYieldMessages(queueName))
                    {
                        i++;
                        System.Console.Write($"In Main Queue : {i} messages");
                        System.Console.CursorLeft = 0;
                    }
                    System.Console.WriteLine();
                    System.Console.WriteLine("Done");
                }

                if (mqopt == MqOptionEnum.GetAllAndClean)
                {
                    int i = 0;
                    foreach (string s in domainModel.GetYieldMessages(queueName))
                    {
                        i++;
                        System.Console.WriteLine($"Message : {s}");
                    }
                    System.Console.WriteLine($"In Main Queue : {i} messages");
                }

                if (mqopt == MqOptionEnum.OnlyRead)
                {
                    int i = 0;
                    foreach (string s in domainModel.ReadYieldMessages(queueName))
                    {
                        i++;
                        System.Console.WriteLine($"Message : {s}");
                    }

                    System.Console.WriteLine($"In Main Queue : {i} messages");
                }


            }
        }

    }
}
