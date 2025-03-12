using System;

namespace Kharazmi.AspNetMvc.Core.Managers.Notification
{
    [Serializable]
    public class NotificationModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public NotificationType ToastType { get; set; }
        public bool IsSticky { get; set; }
    }
}