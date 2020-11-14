using GGsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;

namespace GGsWeb.Controllers
{
    public class HomeController : Controller
    {
        private const string url = "https://localhost:44316/";
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
            if (ModelState.IsValid)
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
                // Verify confirmed password
                if (model.password == model.confirmPassword)
                {
                    // Create and map new user to be added to DB
                    User newUser = new User();
                    newUser.name = model.name;
                    newUser.email = model.email;
                    newUser.locationId = model.locationId;
                    newUser.type = Models.User.userType.Customer;
                    newUser.password = model.password;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(url);

                        // Serialize user
                        var json = JsonConvert.SerializeObject(newUser);
                        var data = new StringContent(json, Encoding.UTF8, "application/json");

                        // Post method to add to db
                        var response = client.PostAsync("user/add", data);
                        response.Wait();

                        var result = response.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            // Successful add
                            HttpContext.Session.SetObject("User", newUser);
                            return RedirectToAction("GetInventory", "Customer");
                        }
                    }
                }
                else
                {
                    // TODO: Failed sign in
                }
            }
            return View(model);
        }
        [HttpGet]
        public ViewResult SignUp()
        {
            var model = new SignUpViewModel();
            
            // Get list of locations
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync("location/getAll");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var jsonString = result.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var locations = JsonConvert.DeserializeObject<List<Location>>(jsonString.Result);
                    var locationOptions = new List<SelectListItem>();
                    foreach(var l in locations)
                    {
                        locationOptions.Add(new SelectListItem { Selected = false, Text = $"{l.city}, {l.state}", Value = l.id.ToString() });
                    }
                    model.locationOptions = locationOptions;
                }
            }
            
            return View(model);
        }
        public IActionResult SignOut()
        {
            // Clear session data and redirect to login page
            HttpContext.Session.Clear();

            return View("Login");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
