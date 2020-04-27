using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigTool
{
    public class ConfigToolLog
    {
        private static Action<string> logInfoAction;
        private static Action<string> logErrorAction;
        public static void Init(Action<string> logInfoActionArg, Action<string> logErrorActionArg)
        {
            logInfoAction = logInfoActionArg;
            logErrorAction = logErrorActionArg;
        }

        public static void LogInfo(string str)
        {
            if (logInfoAction != null)
                logInfoAction(str);
        }

        public static void LogError(string str)
        {
            if (logErrorAction != null)
                logErrorAction(str);
        }


    }
}
