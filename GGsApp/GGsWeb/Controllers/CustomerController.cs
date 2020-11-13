using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GGsWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public IActionResult AddItemToCart(VideoGame videoGame)
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
            user.cart.totalCost += (videoGame.cost * item.quantity);
            user.cart.cartItems.Add(item);
            HttpContext.Session.SetObject("User", user);
            return View("GetInventory", user.location.inventory);
        }
        public IActionResult AddCustomer()
        {
            return View();
        }

        public IActionResult ViewCart()
        {
            user = HttpContext.Session.GetObject<User>("User");
            if (user == null)
                return RedirectToAction("Login", "Home");
            return View(user.cart);
        }
        public IActionResult AddOrder()
        {
            user = HttpContext.Session.GetObject<User>("User");
        }
    }
}
