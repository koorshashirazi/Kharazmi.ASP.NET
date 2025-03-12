using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Mvc.Utility.Core.Extensions
{
    /// <summary>Encapsulates an error from the identity subsystem.</summary>
    public class IdentityError
    {
        /// <summary>Gets or sets the code for this error.</summary>
        /// <value>The code for this error.</value>
        public string Code { get; set; }

        /// <summary>Gets or sets the description for this error.</summary>
        /// <value>The description for this error.</value>
        public string Description { get; set; }
    }
    public static partial class Common
    {
        /// <summary>
        /// 
        /// </summary>
        public const string LineLogSeparator =
            @"===================================================================================";


        /// <summary>
        /// 
        /// </summary>
        public class StackFrameModel
        {
            /// <summary> </summary>
            public string FileName { get; set; }

            /// <summary></summary>
            public string ClassName { get; set; }


            /// <summary></summary>
            public string MethodName { get; set; }

            /// <summary></summary>
            public int LinNumber { get; set; }
        }

        /// <summary></summary>
        public class DetailsError
        {
            /// <summary></summary>
            public string CurrentDate { get; set; }

            /// <summary> </summary>
            public IEnumerable<StackFrameModel> StackFrameModel { get; set; }

            /// <summary> </summary>
            public IEnumerable<IdentityError> ExceptionErrors { get; set; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<IdentityError> CollectExceptionIncludeInnerException<T>(this T e)
            where T : Exception, new()
        {
            var exceptionErrors = new List<IdentityError>();
            while (true)
            {
                if (e == null) return exceptionErrors;
                exceptionErrors.Add(new IdentityError {Code = typeof(T).Name, Description = e.Message});
                if (e.InnerException == null) return exceptionErrors;
                exceptionErrors.Add(new IdentityError
                    {Code = e.GetType().Name, Description = "\r\nInnerException: " + e.InnerException.Message});
                e = (T) e.InnerException;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CheckArgumentIsNull(this object o, string name)
        {
            switch (o)
            {
                case string objString when objString.IsEmpty():
                    throw new ArgumentNullException(name).WithDetailsJsonException();
                case null:
                    throw new ArgumentNullException(name).WithDetailsJsonException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="name"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T CheckArgumentIsNull<T>(this T o, string name) where T : class
        {
            return o ?? throw new ArgumentNullException(name).WithDetailsJsonException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T WithDetailsJsonException<T>(this T exception) where T : Exception, new()
        {
            var stacktrace = (new StackTrace(exception, true).GetFrames() ??
                              new[] {new StackFrame(1, true)})
                .Where(x => x.GetFileName() != null);

            var stackFrameModels = (from stackFrame in stacktrace
                let methodBase = stackFrame.GetMethod()
                let fileName = stackFrame.GetFileName()
                let lineNumber = stackFrame.GetFileLineNumber()
                let memberInfo = methodBase?.DeclaringType
                select new StackFrameModel
                {
                    FileName = fileName,
                    ClassName = memberInfo?.FullName,
                    MethodName = memberInfo?.Name,
                    LinNumber = lineNumber
                }).ToList();

            var details = new DetailsError
            {
                StackFrameModel = stackFrameModels,
                CurrentDate = DateTime.Now.ToShortDateString(),
                ExceptionErrors = exception.CollectExceptionIncludeInnerException()
            };

            var jsonDetails = JsonConvert.SerializeObject(details, Formatting.Indented, new JsonSerializerSettings
            {
                MaxDepth = 10,
                NullValueHandling = NullValueHandling.Ignore
            });

            return (T) Activator.CreateInstance(typeof(T), jsonDetails);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T WithDetailsException<T>(this T exception) where T : Exception, new()
        {
            var exceptionMessage = new StringBuilder();
            var frame = new StackFrame(1, true);
            var methodBase = frame.GetMethod();
            var fileName = frame.GetFileName();
            var lineNumber = frame.GetFileLineNumber();
            var currentDateTime = DateTime.Now.ToShortDateString();

            var memberInfo = methodBase?.DeclaringType;
            var exceptionErrors = exception.CollectExceptionIncludeInnerException();

            exceptionMessage.Append(LineLogSeparator).AppendLine();

            exceptionMessage.Append($"Start an error has occurred: {currentDateTime}").AppendLine();

            exceptionMessage.Append($"Exception Type: {typeof(T).Name}").AppendLine();

            exceptionMessage.Append($"Class: {memberInfo?.FullName ?? ""}").AppendLine();
            exceptionMessage.Append($"Method: {memberInfo?.Name}").AppendLine();
            exceptionMessage.Append($"FileName: {fileName}").AppendLine();
            exceptionMessage.Append($"LineNumber: {lineNumber}").AppendLine();


            foreach (var identityError in exceptionErrors)
            {
                exceptionMessage.Append($"Exception Message:").AppendLine();
                exceptionMessage.Append($"Exception Message Code: {identityError.Code}").AppendLine();
                exceptionMessage.Append($"Exception Message Description: {identityError.Description}").AppendLine();
            }

            exceptionMessage.Append($"End of exception: {LineLogSeparator}").AppendLine();


            return (T) Activator.CreateInstance(typeof(T), exceptionMessage.ToString());
        }
    }
}