using GGsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using GGsWeb.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Diagnostics.Eventing.Reader;

namespace GGsWeb.Controllers
{
    public class HomeController : Controller
    {
        const string url = "https://localhost:44316/";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public ViewResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"user/get?email={model.email}");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();

                        var verifiedUser = JsonConvert.DeserializeObject<User>(jsonString.Result);

                        if (verifiedUser.password == model.password && verifiedUser.email == model.email)
                        {
                            HttpContext.Session.SetObject("User", verifiedUser);
                            return RedirectToAction("GetInventory", "Customer");
                        }
                        else
                        {
                            ModelState.AddModelError("Error", "Invalid information");
                            return View(model);
                        }
                    }
                }
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult SignUp(SignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.password == model.confirmPassword)
                {
                    // Get user information
                }
                //using (var client = new HttpClient())
                //{
                    //client.BaseAddress = new Uri(url);
                    //var response = client.GetAsync($"user/get?email={model.email}");
                    //response.Wait();

                    //var result = response.Result;
                    //if (result.IsSuccessStatusCode)
                    //{
                    //    var jsonString = result.Content.ReadAsStringAsync();
                    //    jsonString.Wait();

                    //    var verifiedUser = JsonConvert.DeserializeObject<User>(jsonString.Result);

                    //    if (verifiedUser.password == model.password && verifiedUser.email == model.email)
                    //    {
                    //        //HttpContext.Session.Set<User>("CurrentUser", user);
                    //        return RedirectToAction("Index", "Customer");
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelError("Error", "Invalid information");
                    //        return View(model);
                    //    }
                    //}
                //}
            }
            return View(model);
        }
        [HttpGet]
        public ViewResult SignUp()
        {
            var model = new SignUpViewModel();
            return View(model);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
