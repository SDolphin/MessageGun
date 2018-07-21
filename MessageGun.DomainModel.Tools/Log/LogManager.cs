using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

namespace MessageGun.DomainModel.Tools.Log
{
    public static class LogManager
    {
        private static ILoggerFactory loggerFactory;

        private static IDictionary<string, ILogger> loggers = new ConcurrentDictionary<string, ILogger>();

        private static bool appendDebug = false;
        private static bool appendConsole = true;
        private static bool appendFile = true;

        private static string defaultDir = "Logs";
        private static string fileName = "log-{Date}.txt";

        static LogManager()
        {
            loggerFactory = new LoggerFactory();

            if (AppendConsole)
            {
                loggerFactory = loggerFactory.AddConsole();
            }

            if (AppendDebug)
            {
                loggerFactory = loggerFactory.AddDebug();
            }

            if (AppendFile)
            {
                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), defaultDir);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                loggerFactory.AddFile(Path.Combine(directoryPath, fileName));
            }
        }

        public static bool AppendDebug
        {
            get { return appendDebug; }
            set { appendDebug = value; }
        }

        public static bool AppendConsole
        {
            get { return appendConsole; }
            set { appendConsole = value; }
        }

        public static bool AppendFile
        {
            get { return appendFile; }
            set { appendFile = value; }
        }

        public static string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public static ILogger GetLogger(Type type)
        {
            if (!loggers.ContainsKey(type.Name))
            {
                loggers.Add(type.Name, loggerFactory.CreateLogger(type));
            }

            return loggers[type.Name];
        }

        public static ILogger GetFileLogger(string identity)
        {

            if (!loggers.ContainsKey(identity))
            {
                LoggerFactory factory = new LoggerFactory();

                string directoryPath = Path.Combine(Directory.GetCurrentDirectory(), defaultDir);

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                factory.AddFile(Path.Combine(directoryPath, string.Format("{0}.txt", identity)));
                loggers.Add(identity, factory.CreateLogger(identity));
            }

            return loggers[identity];
        }

    }
}
