using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GGsWeb.Models;
using GiantBomb.Api;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GGsWeb.Controllers
{
    public class ManagerController : Controller
    {
        const string url = "https://localhost:44316/"; 
        private User user;
        private readonly IConfiguration config;
        public ManagerController(IConfiguration configuration)
        {
            config = configuration;
        }
        public IActionResult GetInventory(int locationId)
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
                return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            { 
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    // Get all locations for select list
                    var response = client.GetAsync("location/getAll");
                    response.Wait();

                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        var locations = JsonConvert.DeserializeObject<List<Location>>(jsonString.Result);
                        var locationOptions = new List<SelectListItem>();
                        foreach (var l in locations)
                        {
                            locationOptions.Add(new SelectListItem { Selected = false, Text = $"{l.city}, {l.state}", Value = l.id.ToString() });
                        }
                        ViewBag.locationOptions = locationOptions;
                    }

                    // Get inventory items at location
                    response = client.GetAsync($"inventoryitem/get/{locationId}");
                    response.Wait();

                    result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();

                        var model = JsonConvert.DeserializeObject<List<InventoryItem>>(jsonString.Result);
                        return View(model);
                    }
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult EditInventoryItem(int locationId, int videoGameId)
        {
            if (ModelState.IsValid)
            {
                // Get inventory item
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"inventoryitem/get/{locationId}/{videoGameId}");
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();

                        var model = JsonConvert.DeserializeObject<InventoryItem>(jsonString.Result);
                        return View(model);
                    }
                }
            }
            return View();
        }
        [HttpPost]
        public IActionResult EditInventoryItem(InventoryItem item)
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
                return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var json = JsonConvert.SerializeObject(item);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = client.PutAsync("inventoryitem/update", data);
                    response.Wait();
                    var result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        // TODO: Give Confirmation 
                        return RedirectToAction("GetInventory", new { locationId = 1 });
                    }
                }
            }
            return RedirectToAction("GetInventory", new { locationId = 1 });
        }
        [HttpGet]
        public IActionResult ViewInventoryItems(string searchString)
        {
            // Get API key:
            string apikey = config.GetConnectionString("GiantBombAPI");
            var giantBomb = new GiantBombRestClient(apikey);
            if (!String.IsNullOrEmpty(searchString))
            {
                var results = giantBomb.SearchForAllGames(searchString);
                return View(results);
            }
            return View(new List<GiantBomb.Api.Model.Game>());
        }
        [HttpGet]
        public IActionResult AddInventoryItem(string name, int id)
        {
            if (ModelState.IsValid)
            {
                InventoryItemViewModel model = new InventoryItemViewModel();
                model.name = name;
                model.apiId = id;

                // Get API key:
                string apikey = config.GetConnectionString("GiantBombAPI");
                var giantBomb = new GiantBombRestClient(apikey);
                var game = giantBomb.GetGame(id);
                model.imageURL = game.Image.SmallUrl;

                // Get Locations
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
                        foreach (var l in locations)
                        {
                            locationOptions.Add(new SelectListItem { Selected = false, Text = $"{l.street}, {l.city}, {l.state}, {l.zipCode}", Value = l.id.ToString() });
                        }
                        ViewBag.locationOptions = locationOptions;
                    }
                }
                return View(model);
            }
            return View();
        }
        [HttpPost]
        public IActionResult AddInventoryItem(InventoryItemViewModel model)
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
                return RedirectToAction("Login", "Home");
            // Create Video game
            VideoGame newVideoGame = new VideoGame();
            newVideoGame.name = model.name;
            newVideoGame.platform = model.platform;
            newVideoGame.esrb = model.esrb;
            newVideoGame.cost = model.cost;
            newVideoGame.description = model.description;
            newVideoGame.apiId = model.apiId;
            newVideoGame.imageURL = model.imageURL;

            // Add video game to database
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var json = JsonConvert.SerializeObject(newVideoGame);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync("videogame/add", data);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    // Successfully added new video game
                    // Get videogame back from db to get id value
                    response = client.GetAsync($"videogame/get/name?name={newVideoGame.name}");
                    response.Wait();
                    result = response.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var jsonString = result.Content.ReadAsStringAsync();
                        jsonString.Wait();
                        newVideoGame = JsonConvert.DeserializeObject<VideoGame>(jsonString.Result);

                        // Create new inventory item and add to DB
                        InventoryItem newItem = new InventoryItem();
                        newItem.locationId = model.locationId;
                        newItem.quantity = model.quantity;
                        newItem.videoGameId = newVideoGame.id;

                        json = JsonConvert.SerializeObject(newItem);
                        data = new StringContent(json, Encoding.UTF8, "application/json");
                        response = client.PostAsync("inventoryitem/add", data);
                        response.Wait();
                        result = response.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            // Successfully added inventory item
                            // TODO: add confirmation message
                            return RedirectToAction("GetInventory", new { locationId = user.locationId });
                        }
                    }
                }
                // Failed
                return RedirectToAction("GetInventory", user.locationId);
            }
        }
    }
}
