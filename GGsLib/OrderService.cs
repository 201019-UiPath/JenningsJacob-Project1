using GGsDB.Repos;
using GGsDB.Models;
using System.Collections.Generic;
using System;

namespace GGsLib
{
    public class OrderService
    {
        IOrderRepo repo;
        public OrderService(IOrderRepo repo)
        {
            this.repo = repo;
        }
        public void AddOrder(Order order)
        {
            repo.AddOrder(order);
        }
        public void DeleteOrder(Order order)
        {
            repo.DeleteOrder(order);
        }
        public List<Order> GetAllOrdersByLocationId(int id)
        {
            return repo.GetAllOrdersByLocationId(id);
        }
        public List<Order> GetAllOrdersByUserId(int id)
        {
            return repo.GetAllOrdersByUserId(id);
        }
        public Order GetOrderByDate(DateTime date)
        {
            return repo.GetOrderByDate(date);
        }
        public Order GetOrderById(int id)
        {
            return repo.GetOrderById(id);
        }
        public Order GetOrderByLocationId(int id)
        {
            return repo.GetOrderByLocationId(id);
        }
        public Order GetOrderByUserId(int id)
        {
            return repo.GetOrderByUserId(id);
        }
        public List<Order> GetAllOrdersDateAsc(int userId)
        {
            return repo.GetAllOrdersDateAsc(userId);
        }
        public List<Order> GetAllOrdersDateDesc(int userId)
        {
            return repo.GetAllOrdersDateDesc(userId);
        }
        public List<Order> GetAllOrdersPriceAsc(int userId)
        {
            return repo.GetAllOrdersPriceAsc(userId);
        }
        public List<Order> GetAllOrdersPriceDesc(int userId)
        {
            return repo.GetAllOrdersPriceDesc(userId);
        }
        public List<Order> GetAllLocationOrdersDateAsc(int locatoinId)
        {
            return repo.GetAllLocationOrdersDateAsc(locatoinId);
        }
        public List<Order> GetAllLocationOrdersDateDesc(int locatoinId)
        {
            return repo.GetAllLocationOrdersDateDesc(locatoinId);
        }
        public List<Order> GetAllLocationOrdersPriceAsc(int locatoinId)
        {
            return repo.GetAllLocationOrdersPriceAsc(locatoinId);
        }
        public List<Order> GetAllLocationOrdersPriceDesc(int locatoinId)
        {
            return repo.GetAllLocationOrdersPriceDesc(locatoinId);
        }
        public Order UpdateOrderCost(Order order, decimal totalCost)
        {
            return repo.UpdateOrderCost(order, totalCost);
        }
        /// <summary>
        /// Prepares and completes order while updating appropriate tables in the database
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cartService"></param>
        /// <param name="cartItemService"></param>
        /// <param name="videoGameService"></param>
        /// <param name="lineItemService"></param>
        /// <param name="inventoryItemService"></param>
        public Order MakePurchase(User user, VideoGameService videoGameService, LineItemService lineItemService, InventoryItemService inventoryItemService)
        {
            // Create new order object to be added to DB
            Order order = new Order();
            decimal totalCost = 0;
            order.userId = user.id;
            order.locationId = user.locationId;
            order.orderDate = DateTime.Now;
            AddOrder(order);
            
            // Get that order back with generated id
            Order newOrder = GetOrderByDate(order.orderDate);
            order.id = newOrder.id;

            foreach (var item in user.cart.cartItems)
            {
                // Get video game from user cart and create new line items to be added to DB
                VideoGame videoGame = item.videoGame;
                LineItem lineItem = new LineItem();
                lineItem.orderId = newOrder.id;
                lineItem.videoGameId = item.videoGameId;
                lineItem.cost = videoGame.cost;
                lineItem.quantity = item.quantity;

                totalCost += (videoGame.cost * item.quantity);

                lineItemService.AddLineItem(lineItem);

                // Remove item from inventory
                InventoryItem inventoryItem = inventoryItemService.GetInventoryItem(user.locationId, videoGame.id);
                inventoryItemService.DiminishInventoryItem(inventoryItem, item.quantity);
            }
            // Clear user cart and update order cost
            user.cart.cartItems.Clear();
            newOrder = UpdateOrderCost(order, totalCost);
            return newOrder;
        }
    }
}