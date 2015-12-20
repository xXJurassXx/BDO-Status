using System;
using System.Diagnostics;
using System.Threading;
using Commons.Threading;
using NLog;
using NLog.Config;
using NLog.Targets;
using WorldServer.Emu;
using WorldServer.Emu.Networking;

namespace WorldServer
{
    internal class WorldServer
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        private static readonly SingleThreadActor GlobalActor = new SingleThreadActor();
        private static readonly ManualResetEvent ThreadBlock = new ManualResetEvent(false);

        static void Main()
        {
            #region Console options
            Console.CancelKeyPress += CancelEventHandler;

            Console.Title = "BDO EMU || World server";

            #endregion

            Console.WriteLine("Authors:\n Sagara\n InCube\n");

            LogManager.Configuration = NLogConfiguration;

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;

            var time = Stopwatch.StartNew();

            GlobalActor.Initialize();

            InitializeServices();

            Log.Info($"Application successfully started at {(time.ElapsedMilliseconds / (float)1000)} seconds");

            ThreadBlock.WaitOne();
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            UninitializeServices();

            GlobalActor.Shutdown();

            Log.Fatal($"Fatal exception occured!\n{e.ExceptionObject}");

            Console.ReadKey();
        }

        private static void CancelEventHandler(object sender, ConsoleCancelEventArgs e)
        {
            UninitializeServices();

            GlobalActor.Shutdown();
        }

        private static void InitializeServices()
        {
            Core.Act(s =>
            {
                s.ReloadProcessors();
                s.AuthProcessor.OnLoad(null);
                s.LobbyProcessor.OnLoad(null);
            });

            NetworkService.Initialize();
        }

        private static void UninitializeServices()
        {
            Core.Act(s =>
            {
                s.AuthProcessor.OnUnload();
                s.LobbyProcessor.OnUnload();
            });
        }

        #region NLogConfiguration

        public static LoggingConfiguration NLogConfiguration
        {
            get
            {
                var config = new LoggingConfiguration();

                var consoleTarget = new ColoredConsoleTarget
                {
                    Layout =
                        "${time} | ${message}${onexception:${newline}EXCEPTION OCCURRED${newline}${exception:format=tostring}}",
                    UseDefaultRowHighlightingRules = false
                };
                config.AddTarget("console", consoleTarget);

                var fileTarget = new FileTarget
                {
                    Layout =
                        "${time} | ${message}${onexception:${newline}EXCEPTION OCCURRED${newline}${exception:format=tostring}}",
                    FileName = "${basedir}/logs/${shortdate}/${level}.txt"
                };

                var allfileTarget = new FileTarget
                {
                    Layout =
                        "${time} | ${level} | ${message}${onexception:${newline}EXCEPTION OCCURRED${newline}${exception:format=tostring}}",
                    FileName = "${basedir}/logs/${shortdate}/all_out.txt"
                };

                config.AddTarget("file", fileTarget);
                config.AddTarget("all_out", allfileTarget);

                consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Debug",
                    ConsoleOutputColor.DarkGray,
                    ConsoleOutputColor.Black));
                consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Info",
                    ConsoleOutputColor.Green,
                    ConsoleOutputColor.Black));
                consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Warn",
                    ConsoleOutputColor.Yellow,
                    ConsoleOutputColor.Black));
                consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Error",
                    ConsoleOutputColor.Red,
                    ConsoleOutputColor.Black));
                consoleTarget.RowHighlightingRules.Add(new ConsoleRowHighlightingRule("level == LogLevel.Fatal",
                    ConsoleOutputColor.Red,
                    ConsoleOutputColor.White));

                var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
                config.LoggingRules.Add(rule1);

                var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
                config.LoggingRules.Add(rule2);

                var rule3 = new LoggingRule("*", LogLevel.Debug, allfileTarget);
                config.LoggingRules.Add(rule3);

                return config;
            }
        }

        #endregion
    }
}
