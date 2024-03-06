using Lesson1.Data;
using Lesson1.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace Lesson1.Middleware
{
    public enum LogAction { Create, Update, Delete }
    public class Logger<T>
    {
        const string WAS_ADDED = "was added by ";
        const string WAS_EDITED = "was edited by ";
        const string WAS_REMOVED = "was removed by ";
        const string AT_TIME = $" : ";
        
        public string PrepareLogText(T entity, User user, LogAction action)
        {
            string text = "";

            if (entity is Product)
                text += (entity as Product)?.Name;
            if(entity is User)
                text += (entity as User)?.Login;

            switch (action)
            {
                case LogAction.Create:
                    text += WAS_ADDED;
                    break;
                case LogAction.Update:
                    text += WAS_EDITED;
                    break;
                case LogAction.Delete:
                    text += WAS_REMOVED;
                    break;
            }

            text += $"{user.Login}{AT_TIME}{DateTime.Now}\n";
            return text;
        }

    }

    interface ILoggerCustom<T>
    {
        const string FileAdress = "EntitiesLog.txt";
        public Logger<T> Logger { get; set; }
        public void Log<T>(T entity, User user, LogAction logAction);
    }
}
