using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using MQ.MClientModel;

using MQ.MDomainModel;
using MQ.MClientModel.Common;

using MessageGun.DomainModel;
using MessageGun.DomainModel.DB;

//using MessageGun.DomainModel.Tools.Converters;

using System.Threading;
using System.Text.RegularExpressions;
using MessageGun.DomainModel.DB.Common;
using MessageGun.DomainModel.DB.Common.Interfaces;

using MessageGun.Console.Options;
using System.Configuration;
using CommandLine;

using MessageGun.DomainModel.Tools.Converters;


using WhatsApp.Waboxapp;
using WhatsApp.Waboxapp.Common;



namespace MessageGun.Console
{
    public class Program
    {

        public static void Main(string[] args)
        {
            //Waboxapp asd = new Waboxapp(new WabCon("e707b32d1ea8caab83838e45c1a6f6cb5ad71bf8287de", "79169557171"));
            //var asdd = asd.SendMessage(1234, "79055728225", "(.)(.)");


            //MqXmlConverter mqXmlConverter = new MqXmlConverter();

           // return;

            

            CmdOptions cmdOptions = ParseCommandArgs(args);
            if (cmdOptions != null)
            {
               
                    StartMessageGun(cmdOptions);

            }


        }



        private static CmdOptions ParseCommandArgs(string[] args)
        {
            CmdOptions cmdOptions = new CmdOptions();

            try
            {
                var parsedArguments = CommandLine.Parser.Default.ParseArguments<CmdOptions>(args).WithParsed<CmdOptions>(opts =>
                {
                    cmdOptions = opts;
                });

                if (parsedArguments.Tag == ParserResultType.Parsed)
                {
                    return cmdOptions;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Err");
                return null;
            }
           

            
        }


        private static void StartMessageGun(CmdOptions cmdOptions)
        {

            if (cmdOptions.MqOption != null && cmdOptions.NewConnectionInfo != null)
            {
                System.Console.WriteLine(cmdOptions.HelpInfo);
            }

            if (cmdOptions.MqOption != null)
            {
                if (CmdOptions.MqOptionStringCheck(cmdOptions.MqOption) != MqOptionEnum.Error)
                {
                    ConsoleCommands.MqOption(CmdOptions.MqOptionStringCheck(cmdOptions.MqOption), MGOptions.Mq.MqProp, MGOptions.Mq.QueueName);
                    return;
                }
                else
                {
                    System.Console.WriteLine("Error in argument of Mq option");
                    return;
                }
            }

            bool newConn = MGOptions.Telegram.NewConnection;
            if (cmdOptions.NewConnectionInfo != null)
            {
                if (CmdOptions.ConnectionStringCheck(cmdOptions.NewConnectionInfo))
                {
                    newConn = Convert.ToBoolean(cmdOptions.NewConnectionInfo);
                }
                else
                {
                    System.Console.WriteLine("Error : (-n /--new-connection requieres true or false)");
                    return;
                }
            }


            System.Console.WriteLine("Message Gun starting...");
            System.Console.WriteLine("Press 'Escape' for exit\n");

            //return;



            newConn = true;

            DomainModel.DomainModel domainModel = new DomainModel.DomainModel(MGOptions.DataBase.DBProperties);

            domainModel.StartTeleDomain(MGOptions.Telegram.Api_Id,
                MGOptions.Telegram.Api_hash,
                MGOptions.Telegram.PhoneNumber,
                PasswordFunc,
                newConn,
                MGOptions.Telegram.TeleInterval);

            // domainModel.StartWhatsAppDomain("e707b32d1ea8caab83838e45c1a6f6cb5ad71bf8287de", "79169557171");

            domainModel.StartMqDomain(MGOptions.Mq.MqProp,
                MGOptions.Mq.QueueName,
                MGOptions.Mq.MqInterval);

            


            WaitForExit();

            domainModel.Dispose();


        }


        private static void WaitForExit()
        {
            ConsoleKey pressedKey;
            
            do
            {
                pressedKey = System.Console.ReadKey().Key;
            }
            while (pressedKey != ConsoleKey.Escape);
        }

        private static string PasswordFunc()
        {
            System.Console.WriteLine("Введите пароль");
            return System.Console.ReadLine();
        }


      
    }
}
