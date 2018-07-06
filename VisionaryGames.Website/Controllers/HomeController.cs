﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VisionaryGames.Website.Models;
using PhoenixRising.InternalAPI.App.MailList;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace VisionaryGames.Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Subscribe model)
        {
            if (ModelState.IsValid)
            {
                string connection = "";
                var appAccessToken = "";

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
