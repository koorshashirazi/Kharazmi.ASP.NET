using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Mvc.Utility.Core.Constraints;

namespace Mvc.Utility.Core.Managers.Notification
{
    [Serializable]
    public sealed class NotificationManager
    {
        private NotificationManager()
        {
            ToastMessages = new List<NotificationModel>();
            ShowNewestOnTop = false;
            ShowCloseButton = false;
        }

        public static NotificationManager Instance => new NotificationManager();

        public bool ShowNewestOnTop { get; set; }
        public bool ShowCloseButton { get; set; }

        public List<NotificationModel> ToastMessages { get; set; }

        public NotificationModel SendNotification(TempDataDictionary tempData, string title, string message
            , NotificationType toastType = NotificationType.Info)
        {
            if (tempData == null) throw new ArgumentNullException(nameof(tempData));

            var toast = new NotificationModel
            {
                Title = title, Message = message, ToastType = toastType
            };

            ToastMessages.Add(toast);
            tempData[Constraint.APPLICATION_NOTIFICATION_NAME] = this;
            return toast;
        }

        public string GeneratePartialView()
        {
            var stringPartial = "1: Install toastr from package nuget manager. \n" + "2: Make a partial view.\n" +
                                "3: Add the code below.\n" + "4: Add the partial view created to Layout .\n" +
                                "@using Mvc.Utility.Core.Managers.Notification\n" +
                                "@using Mvc.Utility.Core.Constraints\n" + "@model System.Web.Mvc.TempDataDictionary\n" +
                                "<link href=\"~/Content/toastr.css\" rel=\"stylesheet\" /> \n" +
                                "<script src=\"~/Scripts/toastr.js\"></script>\n" +
                                "<script src=\"~/Scripts/toastr.js\"></script>\n" +
                                "@if(TempData[Constraint.APPLICATION_NOTIFICATION_NAME] != null)\n" + "{\n" +
                                "if (TempData[Constraint.APPLICATION_NOTIFICATION_NAME] is NotificationManager toastr)\n" +
                                "{\n" + "<script type=\"text/javascript\">\n" + "$(document).ready(function() {\n" +
                                "window.toastr.options.closeButton = '@toastr.ShowCloseButton';\n" +
                                "window.toastr.options.newestOnTop = '@toastr.ShowNewestOnTop';\n" +
                                "@foreach(NotificationModel message in toastr.ToastMessages)\n" + "{\n" +
                                "string toastTypeValue = message.ToastType.ToString(\"F\").ToLower();\n" +
                                "@:var optionsOverride = { };\n" + "if (message.IsSticky)\n" + "{\n" +
                                " @:optionsOverride.timeOut = 0;\n" + "@:optionsOverride.extendedTimeout = 0;\n" +
                                "}\n" +
                                "@:window.toastr['@toastTypeValue']('@message.Message', '@message.Title', optionsOverride);\n" +
                                "}\n" + "});\n" + "</script>\n" + "}\n" +
                                "TempData[Constraint.APPLICATION_NOTIFICATION_NAME] = null;\n" + "}\n";


            return stringPartial;
        }
    }
}