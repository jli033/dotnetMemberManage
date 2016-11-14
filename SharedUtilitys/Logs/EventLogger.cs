using System;
using PacificSystem.Utility;

namespace SharedUtilitys.Logs
{
    public class EventLogger
    {
        public static EventLogger GetInstance()
        {
            return new EventLogger();
        }

        public void WriteInfo(string sourceName, string message)
        {
            EventLogManager.WriteInfomationLogEntry(sourceName, message);
        }

        public void WriteError(string sourceName, string message)
        {
            var detail = String.Format(@"{1}{0}{0}StackTrace:{0}", Environment.NewLine, message);
            EventLogManager.WriteErrorLogEntry(sourceName, new EventException(detail), sourceName, 1);
        }

        public void WriteError(string sourceName, string message, Exception exception)
        {
            var detail = String.Format(@"{1}{0}{0}StackTrace:{0}{2}", Environment.NewLine, message, exception.StackTrace);
            EventLogManager.WriteErrorLogEntry(sourceName, new EventException(detail), sourceName, 1);
        }
    }
}
