using KalumManagement.Models;

namespace KalumManagement.Utils
{
    public interface IUtilities
    {
        public void LogPrint(AppLog appLog, int responseCode, string message, string typeLog, HttpRequest request);
    }
}