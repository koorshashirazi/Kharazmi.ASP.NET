using System;
using System.Diagnostics;
using System.Text;
using System.Web;

namespace Kharazmi.AspNetMvc.Core.Helpers
{
    public static class ExceptionHelper
    {
        public static Exception ThrowException<T>(string message, params string[] parametersName) where T : Exception
        {
            var exceptionMessage = new StringBuilder();
            var frame = new StackFrame(1, true);
            var methodBase = frame.GetMethod();
            var fileName = frame.GetFileName();
            var lineNumber = frame.GetFileLineNumber();

            exceptionMessage.Append($"ExceptionType: {typeof(T).Name}").AppendLine();

            var memberInfo = methodBase.DeclaringType;

            if (memberInfo != null)
            {
                exceptionMessage.Append($"Class: {memberInfo.FullName}").AppendLine();
                exceptionMessage.Append($"Method: {memberInfo.Name}").AppendLine();
            }

            exceptionMessage.Append($"FileName: {fileName}").AppendLine();
            exceptionMessage.Append($"LineNumber: {lineNumber}").AppendLine();

            foreach (var param in parametersName) exceptionMessage.Append($"ParameterMethod: {param}").AppendLine();

            if (!string.IsNullOrWhiteSpace(message))
                exceptionMessage.Append($"Exception Message: {message}").AppendLine();

            return (Exception) Activator.CreateInstance(typeof(T), exceptionMessage.ToString());
        }

        public static HttpException ThrowHttpException(int statusErrorValid, string message,
            params string[] parametersName)
        {
            var exceptionMessage = new StringBuilder();
            var frame = new StackFrame(1, true);
            var methodBase = frame.GetMethod();
            var fileName = frame.GetFileName();
            var lineNumber = frame.GetFileLineNumber();

            exceptionMessage.Append($"ExceptionType: {typeof(HttpException).Name}").AppendLine();

            var memberInfo = methodBase.DeclaringType;

            if (memberInfo != null)
            {
                exceptionMessage.Append($"Class: {memberInfo.FullName}").AppendLine();
                exceptionMessage.Append($"Method: {memberInfo.Name}").AppendLine();
            }

            exceptionMessage.Append($"FileName: {fileName}").AppendLine();
            exceptionMessage.Append($"LineNumber: {lineNumber}").AppendLine();

            foreach (var param in parametersName) exceptionMessage.Append($"ParameterMethod: {param}").AppendLine();

            if (!string.IsNullOrWhiteSpace(message))
                exceptionMessage.Append($"Exception Message: {message}").AppendLine();

            return new HttpException(statusErrorValid, exceptionMessage.ToString());
        }
    }
}