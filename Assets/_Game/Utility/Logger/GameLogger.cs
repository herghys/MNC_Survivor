using System.Collections;
using System.Collections.Generic;
using SysDiag = System.Diagnostics;

using UnityEngine;

namespace HerghysStudio.Survivor.Utility.Logger
{
    public class GameLogger
    {
        #region Debug Log
        /// <summary>
        /// Logs a message to the Unity Console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        [SysDiag.Conditional("ENABLE_LOGS")]
        public static void Log(object message)
            => Debug.Log(message);

        /// <summary>
        /// Logs a message to the Unity Console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context"> Object to which the message applies.</param>
        [SysDiag.Conditional("ENABLE_LOGS")]
        public static void Log(object message, Object context)
            => Debug.Log(message, context);
        #endregion

        #region Debug Log Formatted
        /// <summary>
        /// Logs a formatted message to the Unity Console.
        /// </summary>
        /// <param name="logType">Type of message e.g. warn or error etc.</param>
        /// <param name="logOptions">Option flags to treat the log message special.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        [SysDiag.Conditional("ENABLE_LOGS")]
        public static void LogFormat(string format, params object[] args)
            => Debug.LogFormat(format, args);

        /// <summary>
        /// Logs a formatted message to the Unity Console.
        /// </summary>
        /// <param name="logType">Type of message e.g. warn or error etc.</param>
        /// <param name="logOptions">Option flags to treat the log message special.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        [SysDiag.Conditional("ENABLE_LOGS")]
        public static void LogFormat(Object context, string format, params object[] args)
            => Debug.LogFormat(context, format, args);

        /// <summary>
        /// Logs a formatted message to the Unity Console.
        /// </summary>
        /// <param name="logType">Type of message e.g. warn or error etc.</param>
        /// <param name="logOptions">Option flags to treat the log message special.</param>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        /// 
        [SysDiag.Conditional("ENABLE_LOGS")]
        public static void LogFormat(LogType logType, LogOption logOptions, Object context, string format, params object[] args)
            => Debug.LogFormat(logType, logOptions, context, format, args);
        #endregion

        #region Error
        /// <summary>
        /// A variant of Debug.Log that logs an error message to the console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        [SysDiag.Conditional("ENABLE_LOGS")]
        public static void LogError(object message)
            => Debug.LogError(message);
        /// <summary>
        /// A variant of Debug.Log that logs an error message to the console.
        /// </summary>
        /// <param name="message">String or object to be converted to string representation for display.</param>
        /// <param name="context">Object to which the message applies.</param>
        [SysDiag.Conditional("ENABLE_LOGS")]
        public static void LogError(object message, Object context)
            => Debug.LogError(message, context);

        /// <summary>
        /// Logs a formatted error message to the Unity console.
        /// </summary>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        [SysDiag.Conditional("ENABLE_LOGS")]
        public static void LogErrorFormat(string format, params object[] args)
            => Debug.LogErrorFormat(format, args);

        /// <summary>
        /// Logs a formatted error message to the Unity console.
        /// </summary>
        /// <param name="context">Object to which the message applies.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">Format arguments.</param>
        [SysDiag.Conditional("ENABLE_LOGS")]
        public static void LogErrorFormat(Object context, string format, params object[] args)
            => Debug.LogErrorFormat(context, format, args);
        #endregion
    }
}