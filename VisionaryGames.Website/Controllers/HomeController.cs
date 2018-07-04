using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VisionaryGames.Website.Models;
using PhoenixRising.InternalAPI.App.MailList;

namespace VisionaryGames.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Subscribe model)
        {
            if (ModelState.IsValid)
            {
                string connection = ConfigurationManager.AppSettings["InternalAPIURL"];
                var appAccessToken = WebUtils.GetVaultSecret("AppConnectionKey");

                SubscribeRequest resetRequest = new SubscribeRequest(connection, appAccessToken, model.Email);
                SubscribeResponse resetResponse = resetRequest.Send();

                //always act like success - don't want people fishing for email addresses
                TempData["Success"] = "You have been subscribed to our newsletter! We promise to only email the most important updates.";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }
    }
}