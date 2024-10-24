#region using
using NLog;
#endregion

namespace FlyMosquito.Common
{
    /// <summary>
    /// nLog使用帮助类
    /// </summary>
    public static class LoggerHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug(string msg) => Logger.Debug(msg);

        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="msg"></param>
        public static void Info(string msg) => Logger.Info(msg);

        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="msg"></param>
        public static void Warn(string msg) => Logger.Warn(msg);

        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg"></param>
        public static void Error(string msg) => Logger.Error(msg);

        /// <summary>
        /// 致命错误
        /// </summary>
        /// <param name="msg"></param>
        public static void Fatal(string msg) => Logger.Fatal(msg);
    }
}
