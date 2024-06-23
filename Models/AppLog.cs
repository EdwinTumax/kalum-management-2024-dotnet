using System.Net;

namespace KalumManagement.Models
{
    public class AppLog
    {
        public string Name {get;set;} = "kalum-management";
        public string HostName {get;set;} = Dns.GetHostName();
        public string ApiKey {get;set;}
        public string Uri {get;set;}
        public int ResponseCode {get;set;}
        public string Method {get;set;}
        public long ResponseTime {get;set;}
        public string Ip {get;set;} = "127.0.0.1";
        public int LogLevel {get;set;}
        public string Message {get;set;}
        public DateTime DateTime {get;set;}
        public string Version {get;set;} = "v1.0.0";        
        public AppLog()
        {
        }
    }
}