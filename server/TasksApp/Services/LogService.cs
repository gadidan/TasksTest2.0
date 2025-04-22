using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasksApp.Services
{
    public class LogService
    {
        public bool Enabled { get; set; } = true;

        public void LogRequest(string method, string path, string developerName)
        {
            if (!Enabled) return;
            var now = getCurrentTime();
            Console.WriteLine($"[{now}] בקשה: {method} {path} | מפתח: {developerName}");
        }

        private string getCurrentTime()
        {
            return DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }

        // can be Error, Fatal, Info, Warn etc. 
        public void Debug(string message, string developerName)
        {
            var now = getCurrentTime();
            Console.WriteLine($"[{now}] : {message} | developer: {developerName}");
        }
    }
}
