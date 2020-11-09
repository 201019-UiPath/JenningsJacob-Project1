using System;
using System.Collections.Generic;
using GGsDB.Models;

namespace GGsDB.Repos
{
    public interface IOrderRepo
    {
        void AddOrder(Order order);
        Order UpdateOrderCost(Order order, decimal totalCost);
        Order GetOrderById(int id);
        Order GetOrderByUserId(int id);
        Order GetOrderByLocationId(int id);
        Order GetOrderByDate(DateTime dateTime);
        List<Order> GetAllOrdersByLocationId(int id);
        List<Order> GetAllOrdersByUserId(int id);
        List<Order> GetAllOrdersDateAsc(int userId);
        List<Order> GetAllOrdersDateDesc(int userId);
        List<Order> GetAllOrdersPriceAsc(int userId);
        List<Order> GetAllOrdersPriceDesc(int userId);
        List<Order> GetAllLocationOrdersDateAsc(int locationId);
        List<Order> GetAllLocationOrdersDateDesc(int locationId);
        List<Order> GetAllLocationOrdersPriceAsc(int locationId);
        List<Order> GetAllLocationOrdersPriceDesc(int locationId);
        void DeleteOrder(Order order);
    }
}