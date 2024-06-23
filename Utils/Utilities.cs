using System.Text.Json;
using KalumManagement.Models;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace KalumManagement.Utils
{
    public class Utilities : IUtilities
    {
        public void LogPrint(AppLog appLog, int responseCode, string message, string typeLog, HttpRequest request)
        {
            if(request != null)
            {
                request.Headers.TryGetValue("Authorization", out StringValues token);
                appLog.Uri = request.Path;
                appLog.ApiKey = !token.IsNullOrEmpty() ? token.ToString().Split(" ")[1].Split(".")[1]:"";
                appLog.Ip = request.Host.ToString();
                appLog.Method = request.Method;
            }
            appLog.ResponseCode = responseCode;
            appLog.Message = message;
            appLog.DateTime = DateTime.Now;
            switch(typeLog.ToUpper())
            {
                case "ERROR":
                    appLog.LogLevel = 40;
                    Log.Error(JsonSerializer.Serialize(appLog));
                    break;
                case "WARNING":
                    appLog.LogLevel = 30;
                    Log.Warning(JsonSerializer.Serialize(appLog));
                    break;
                case "INFORMATION":
                    appLog.LogLevel = 20;
                    Log.Information(JsonSerializer.Serialize(appLog));
                    break;
                case "DEBUG":
                    appLog.LogLevel = 10;
                    Log.Debug(JsonSerializer.Serialize(appLog));
                    break;
            }
            appLog.ResponseTime = Convert.ToInt32(DateTime.Now.ToString("fff")) - appLog.ResponseTime;            
        }

    }
}