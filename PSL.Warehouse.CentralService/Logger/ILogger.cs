﻿using System;

namespace PSL.Warehouse.CentralService.Logger
{
    public interface ILogger<T>
    {

        void Info(string message);
        void Info(string message, Exception exception);
        void Info(Exception exception);

        void Debug(string message);
        void Debug(string message, Exception exception);
        void Debug(Exception exception);

        void Warn(string message);
        void Warn(string message, Exception exception);
        void Warn(Exception exception);

        void Error(string message);
        void Error(string message, Exception exception);
        void Error(Exception exception);
    }
}
