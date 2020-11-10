using GGsDB.Models;
using System;
using System.Collections.Generic;

namespace GGsLib
{
    public interface IOrderService
    {
        void AddOrder(Order order);
        void DeleteOrder(Order order);
        List<Order> GetAllLocationOrdersDateAsc(int locatoinId);
        List<Order> GetAllLocationOrdersDateDesc(int locatoinId);
        List<Order> GetAllLocationOrdersPriceAsc(int locatoinId);
        List<Order> GetAllLocationOrdersPriceDesc(int locatoinId);
        List<Order> GetAllOrdersByLocationId(int id);
        List<Order> GetAllOrdersByUserId(int id);
        List<Order> GetAllOrdersDateAsc(int userId);
        List<Order> GetAllOrdersDateDesc(int userId);
        List<Order> GetAllOrdersPriceAsc(int userId);
        List<Order> GetAllOrdersPriceDesc(int userId);
        Order GetOrderByDate(DateTime date);
        Order GetOrderById(int id);
        Order GetOrderByLocationId(int id);
        Order GetOrderByUserId(int id);
        Order MakePurchase(User user, VideoGameService videoGameService, LineItemService lineItemService, InventoryItemService inventoryItemService);
        Order UpdateOrderCost(Order order, decimal totalCost);
    }
}