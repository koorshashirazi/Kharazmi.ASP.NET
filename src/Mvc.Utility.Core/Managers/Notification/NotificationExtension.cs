using System;
using System.Web.Mvc;
using Mvc.Utility.Core.Constraints;

namespace Mvc.Utility.Core.Managers.Notification
{
    public static class NotificationExtension
    {
        public static NotificationModel SendNotification(this TempDataDictionary tempData, string title, string message,
            NotificationType toastType = NotificationType.Info)
        {
            if (tempData == null) throw new ArgumentNullException(nameof(tempData));

            var toastr = tempData[Constraint.APPLICATION_NOTIFICATION_NAME] as NotificationManager;
            toastr = toastr ?? NotificationManager.Instance;
            var toastMessage = toastr.SendNotification(tempData, title, message, toastType);
            return toastMessage;
        }
    }
}