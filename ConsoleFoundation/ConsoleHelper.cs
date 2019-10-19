using System;
using NLog;

namespace ConsoleFoundation
{
    public static class ConsoleHelper
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void Console(string msg, LogLevel logLevel = null)
        {
            if (logLevel == null)
            {
                logLevel = LogLevel.Info;
            }

            Logger.Log(logLevel, msg);

            var defaultColor = System.Console.ForegroundColor;

            if (logLevel == LogLevel.Error || logLevel == LogLevel.Fatal)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
            }

            if (logLevel == LogLevel.Info)
            {
                System.Console.ForegroundColor = ConsoleColor.Green;
            }

            if (logLevel == LogLevel.Warn)
            {
                System.Console.ForegroundColor = ConsoleColor.Yellow;
            }

            System.Console.WriteLine($"{DateTime.Now:yyyy/MM/dd hh:mm:ss}:{msg}");
            System.Console.ForegroundColor = defaultColor;
        }

        public static void Console(Exception exception)
        {
            var defaultColor = System.Console.ForegroundColor;

            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"{DateTime.Now:yyyy/MM/dd hh:mm:ss}:{exception}");

            Logger.Error(exception);

            System.Console.ForegroundColor = defaultColor;
        }
    }
}