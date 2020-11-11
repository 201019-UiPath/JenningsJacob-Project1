using GGsDB.Models;
using System;
using System.Collections.Generic;

namespace GGsLib
{
    public interface IOrderService
    {
        void AddOrder(Order order);
        void DeleteOrder(Order order);
        List<Order> GetAllOrdersByLocationId(int locationId);
        List<Order> GetAllOrdersByUserId(int userId);
        Order GetOrderByDate(DateTime date);
        void UpdateOrder(Order order);
        Order MakePurchase(User user, VideoGameService videoGameService, LineItemService lineItemService, InventoryItemService inventoryItemService);
    }
}