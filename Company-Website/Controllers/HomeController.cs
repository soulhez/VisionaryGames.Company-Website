using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Company_Website.Models;
using PhoenixRising.InternalAPI.App.MailList;

namespace Company_Website.Controllers
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
                var appAccessToken = ConfigurationManager.AppSettings["AppKey"];

                SubscribeRequest subscribeRequest = new SubscribeRequest(connection, appAccessToken, model.Email);
                SubscribeResponse subscribeResponse = subscribeRequest.Send();
                
                if (subscribeResponse.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    TempData["Success"] = "You have been subscribed to our newsletter!";
                    return RedirectToAction("Index", "Home");
                }
                else if (subscribeResponse.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    TempData["Errors"] = "That email address is already subscribed.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["Errors"] = "There was an error processing your request.";
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return View(model);
            }
        }
    }
}