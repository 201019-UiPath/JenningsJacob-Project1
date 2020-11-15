using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GGsWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;

namespace GGsWeb.Controllers
{
    public class VideoGameController : Controller
    {
        const string url = "https://localhost:44316/";
        private User user;

        /// <summary>
        /// Gets a video game's details
        /// </summary>
        /// <param name="id">ID of the video game you wish to get</param>
        /// <returns>DetailsView</returns>
        public IActionResult Details(int id)
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
            {
                Log.Error("User session was not found");
                return RedirectToAction("Login", "Home");
            }
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"videogame/get?id={id}");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();

                        Log.Information($"Successfully got videogame: {id}");

                        var model = JsonConvert.DeserializeObject<VideoGame>(jsonString.Result);
                        return View(model);
                    }
                }
            }
            Log.Error($"Unsuccessfully got videogame: {id}");
            return View();
        }
    }
}
