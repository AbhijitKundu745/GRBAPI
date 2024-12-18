﻿using log4net;
using PSL.Warehouse.CentralService.Logger;
using System;

namespace PSL.Laundry.CentralService.Logger
{
    public class Log4netLogger<T> : ILogger<T> where T : class
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.Assembly.GetCallingAssembly(), typeof(T).Name);
        public void Debug(string message)
        {
            Log.Debug(message);
        }

        public void Debug(string message, Exception exception)
        {
            Log.Debug(message, exception);
        }

        public void Debug(Exception exception)
        {
            Log.Debug(exception);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Error(string message, Exception exception)
        {
            Log.Error(message, exception);
        }

        public void Error(Exception exception)
        {
            Log.Error(exception);
        }

        public void Info(string message)
        {
            Log.Info(message);
        }

        public void Info(string message, Exception exception)
        {
            Log.Info(message, exception);
        }

        public void Info(Exception exception)
        {
            Log.Info(exception);
        }

        public void Warn(string message)
        {
            Log.Info(message);
        }

        public void Warn(string message, Exception exception)
        {
            Log.Warn(message, exception);
        }

        public void Warn(Exception exception)
        {
            Log.Warn(exception);
        }
    }
}
