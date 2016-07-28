using System;
using System.IO;
using Raven.Server.Config;
using Raven.Server.Utils;

namespace Raven.Server
{
    public class Program
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(Program));

        public static int Main(string[] args)
        {
            WelcomeMessage.Print();

            var configuration = new RavenConfiguration();
            if (args != null)
            {
                configuration.AddCommandLine(args);
            }
            configuration.Initialize();

            using (var server = new RavenServer(configuration))
            {
                try
                {
                    server.Initialize();

                    Console.WriteLine($"Listening to: {string.Join(", ", configuration.Core.ServerUrl)}");
                    Console.WriteLine("Server started, listening to requests...");

                    //TODO: Move the command line options to here
                    while (true)
                    {
                        if (Console.ReadLine() == "q")
                            break;

                        // Console.ForegroundColor++;
                    }
                    Log.Info("Server is shutting down");
                    return 0;
                }
                catch (Exception e)
                {
                    Log.FatalException("Failed to initialize the server", e);
                    Console.WriteLine(e);
                    return -1;
                }
            }
        }
    }

    internal class LogManager
    {
        public static ILog GetLogger(Type type)
        {
            throw new NotImplementedException();
        } public static ILog GetLogger(string type)
        {
            throw new NotImplementedException();
        }
    }

    public interface ILog
    {
        bool IsInfoEnabled { get; }

        bool IsDebugEnabled { get; }

        bool IsWarnEnabled { get; }

        void Log(LogLevel logLevel, Func<string> messageFunc);

        void Log<TException>(LogLevel logLevel, Func<string> messageFunc, TException exception) where TException : Exception;
        bool ShouldLog(LogLevel logLevel);
        void FatalException(string couldNotOpenTheServerStore, Exception p1);
        void Debug(string serverStoreStartedTookMs, params object[] elapsedMilliseconds);
        void Error(string s, Exception exception = null);
        void Info(string initializedServer);
        void WarnException(string failureInDeferredDisposalOfADatabase, Exception p1);
        void ErrorException(string s, Exception exception = null);
        void DebugException(string clientWasDisconnected, Exception ioException);
        void Warn(string s);
        void Warn(string s,  params object[] name);
    }

    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}