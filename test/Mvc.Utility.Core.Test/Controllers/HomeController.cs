﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mvc.Utility.Core.Constraints;
using Mvc.Utility.Core.Managers.Notification;

namespace Mvc.Utility.Core.Test.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var partial = NotificationManager.Instance.GeneratePartialView();
            TempData.SendNotification("Test", "test", NotificationType.Info);

            var f = TempData[Constraint.APPLICATION_NOTIFICATION_NAME];
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}