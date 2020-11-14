using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GGsWeb.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NpgsqlTypes;

namespace GGsWeb.Controllers
{
    public class CustomerController : Controller
    {
        const string url = "https://localhost:44316/";
        private User user;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetInventory()
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
                return RedirectToAction("Login", "Home");
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    var response = client.GetAsync($"inventoryitem/get/{user.locationId}");
                    response.Wait();

                    var result = response.Result;
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
        public IActionResult AddItemToCart(int videoGameId, int quantity)
        {
            VideoGame videoGame = new VideoGame();
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
                return RedirectToAction("Login", "Home");
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var response = client.GetAsync($"videogame/get?id={videoGameId}");
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    var jsonString = result.Content.ReadAsStringAsync();
                    jsonString.Wait();
                    var vg = JsonConvert.DeserializeObject<VideoGame>(jsonString.Result);
                    videoGame = vg;
                }
            }
            CartItem item = new CartItem()
            {
                videoGameId = videoGameId,
                videoGame = videoGame,
                quantity = quantity
            };
            user.cart.totalCost += (videoGame.cost * quantity);
            user.cart.cartItems.Add(item);
            HttpContext.Session.SetObject("User", user);
            return View("GetInventory", user.location.inventory);
        }
        public IActionResult RemoveItemFromCart(VideoGame videoGame)
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
                return RedirectToAction("Login", "Home");
            CartItem item = new CartItem()
            {
                videoGameId = videoGame.id,
                videoGame = videoGame,
                quantity = 1
            };
            user.cart.cartItems.RemoveAll(x => x.videoGameId == item.videoGameId);
            user.cart.totalCost -= (videoGame.cost * item.quantity);
            HttpContext.Session.SetObject("User", user);
            return RedirectToAction("GetCart");
        }
        public IActionResult GetCart()
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
                return RedirectToAction("Login", "Home");
            return View(user.cart);
        }
        [HttpGet]
        public IActionResult EditUser()
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
                return RedirectToAction("Login", "Home");
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
                        locationOptions.Add(new SelectListItem { Selected = false, Text = $"{l.city}, {l.state}", Value = l.id.ToString() });
                    }
                    ViewBag.locationOptions = locationOptions;
                }
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult EditUser(User newUser)
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
                return RedirectToAction("Login", "Home");

            // Map newUser values
            // User decided not to update password, keep it the same
            if (newUser.password == null)
                newUser.password = user.password;
            newUser.location = user.location;
            newUser.orders = user.orders;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                var json = JsonConvert.SerializeObject(newUser);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PutAsync("user/update", data);
                response.Wait();
                var result = response.Result;
                if (result.IsSuccessStatusCode)
                {
                    // Successfully edited user
                    HttpContext.Session.SetObject("User", newUser);
                    return View(newUser);
                }
            }
            return View("GetInventory");
        }
    }
}
