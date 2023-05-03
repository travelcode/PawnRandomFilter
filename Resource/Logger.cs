using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Verse;

namespace Nomadicooer.rimworld.crp
{
    public static class Logger
    {
        private static readonly HashSet<int> _hashMessage = new HashSet<int>();
        private static LoggerLevel _level = LoggerLevel.Debug;
        public static LoggerLevel Level { get => _level; set => _level = value; }
        private static string GetFullMessage(string message, LoggerLevel level, StringColor loggerColor)
        {
            StringBuilder builder = new StringBuilder();
            string callerName = GetCaller(3);
            builder.Append(StringColor.Prefix.GetColorMessage("ConditionalRandomPawns"))
                .Append('[').Append(loggerColor.GetColorMessage(level.ToString())).Append(']')
                .Append('[').Append(callerName).Append(']')
                .Append("===>").Append(loggerColor.GetColorMessage(message));
            return builder.ToString();
        }
        private static string GetCaller(int index)
        {
            StackTrace trace = new StackTrace();
            StackFrame frame = trace.GetFrame(index);
            MethodBase callerMethod = frame.GetMethod();
            string clazzName = callerMethod.DeclaringType.Name;
            string methodName = callerMethod.Name;
            StringBuilder builder = new StringBuilder();
            foreach (var param in callerMethod.GetParameters())
            {
                builder.Append(param.ParameterType.Name).Append(' ').Append(param.Name).Append(',');
            }
            string parameters = builder.ToString().TrimEnd(',');
            builder.Clear();
            builder.Append(clazzName).Append('.').Append(methodName).Append('(').Append(parameters).Append(')');
            return builder.ToString();
        }
        public static void Trace(string message)
        {
            if (_level > LoggerLevel.Trace) return;
            message = GetFullMessage(message, LoggerLevel.Trace, StringColor.Trace);
            Log.Message(message);
        }
        public static void TraceOnce(string message)
        {
            if (_level > LoggerLevel.Trace) return;
            if (_hashMessage.Contains(message.GetHashCode()))
            {
                return;
            }
            _hashMessage.Add(message.GetHashCode());
            message = GetFullMessage(message, LoggerLevel.Trace, StringColor.Trace);
            Log.Message(message);
        }
        public static void Debug(string message)
        {
            if (_level > LoggerLevel.Debug) return;
            message = GetFullMessage(message, LoggerLevel.Debug, StringColor.Debug);
            Log.Message(message);
        }
        public static void DebugOnce(string message)
        {
            if (_level > LoggerLevel.Debug) return;
            if (_hashMessage.Contains(message.GetHashCode()))
            {
                return;
            }
            _hashMessage.Add(message.GetHashCode());
            message = GetFullMessage(message, LoggerLevel.Debug, StringColor.Debug);
            Log.Message(message);
        }
        public static void Info(string message)
        {
            if (_level > LoggerLevel.Info) return;
            message = GetFullMessage(message, LoggerLevel.Info, StringColor.Info);
            Log.Message(message);
        }
        public static void InfoOnce(string message)
        {
            if (_level > LoggerLevel.Info) return;
            if (_hashMessage.Contains(message.GetHashCode()))
            {
                return;
            }
            _hashMessage.Add(message.GetHashCode());
            message = GetFullMessage(message, LoggerLevel.Info, StringColor.Info);
            Log.Message(message);
        }
        public static void Warn(string message)
        {
            if (_level > LoggerLevel.Warn) return;
            message = GetFullMessage(message, LoggerLevel.Warn, StringColor.Warn);
            Log.Warning(message);
        }
        public static void WarnOnce(string message)
        {
            if (_level > LoggerLevel.Warn) return;
            message = GetFullMessage(message, LoggerLevel.Warn, StringColor.Warn);
            Log.WarningOnce(message,message.GetHashCode());
        }
        public static void Error(string message)
        {
            if (_level > LoggerLevel.Error) return;
            message = GetFullMessage(message, LoggerLevel.Error, StringColor.Error);
            Log.Error(message);
        }
        public static void ErrorOnce(string message)
        {
            if (_level > LoggerLevel.Error) return;
            message = GetFullMessage(message, LoggerLevel.Error, StringColor.Error);
            Log.ErrorOnce(message,message.GetHashCode());
        }
        public static void Fatal(string message)
        {
            if (_level > LoggerLevel.Fatal) return;
            message = GetFullMessage(message, LoggerLevel.Fatal, StringColor.Fatal);
            Log.Error(message);
        }
        public static void FatalOnce(string message)
        {
            if (_level > LoggerLevel.Fatal) return;
            message = GetFullMessage(message, LoggerLevel.Fatal, StringColor.Fatal);
            Log.ErrorOnce(message,message.GetHashCode());
        }
    }
}
