using System.IO;
using System.Web.Http.ExceptionHandling;

namespace BloodPressureWebApi.Logger
{
    public class BloodPressureLogger : ExceptionLogger
    { // TODO : log4net ya da nLog kullanılacak loglama zaman kaybı olmasn
        public string LogPath =
            @"D:\Users\mvural\Documents\Visual Studio 2015\Projects\BloodPressure\BloodPressureWebApi\Logger";

        public override void Log(ExceptionLoggerContext context)
        {
            var log = context.Exception.ToString();
            File.WriteAllText(LogPath + "log.txt",log);
        }
    }
}