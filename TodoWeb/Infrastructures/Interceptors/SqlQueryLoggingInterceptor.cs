using System.Data.Common;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace TodoWeb.Infrastructures.Interceptors;

using System;
using System.Diagnostics;
using System.IO;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class SqlQueryLoggingInterceptor : DbCommandInterceptor
{
    private readonly Stopwatch stopwatch = new Stopwatch();

    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        stopwatch.Restart(); // Đặt lại và bắt đầu đo thời gian
        return base.ReaderExecuting(command, eventData, result);
    }

    public override DbDataReader ReaderExecuted(
        DbCommand command, CommandExecutedEventData eventData, DbDataReader result)
    {
        stopwatch.Stop(); // Dừng đo khi câu query đã thực thi xong

        if (stopwatch.ElapsedMilliseconds > 2) // Chỉ log khi mất hơn 2ms
        {
            LogQuery(command.CommandText, stopwatch.ElapsedMilliseconds);
        }

        return base.ReaderExecuted(command, eventData, result);
    }

    private void LogQuery(string query, long executionTime)
    {
        string logPath = "E:\\Hoc\\.Net\\Ok\\TodoWeb\\TodoWeb\\sqllog.txt";
        string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {executionTime} ms | {query}";

        File.AppendAllText(logPath, logMessage + Environment.NewLine);
    }
}

