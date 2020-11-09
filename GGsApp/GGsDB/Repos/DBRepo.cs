using GGsDB.Entities;
using GGsDB.Mappers;
using GGsDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace GGsDB.Repos
{
    /// <summary>
    /// Repository responsible for interfacing directlly with the database
    /// </summary>
    public class DBRepo : IRepo
    {
        private readonly GGsContext context;
        private readonly IMapper mapper;
        public DBRepo(GGsContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        #region User Methods
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">User you wish to add</param>
        public void AddUser(User user)
        {
            context.Users.Add(mapper.ParseUser(user));
            context.SaveChanges();
        }
        /// <summary>
        /// Updates a user in the database
        /// </summary>
        /// <param name="user">Updated user</param>
        public void UpdateUser(User user)
        {
            //var entity = context.Users.FirstOrDefault(i => i.Id == user.id);
            //if (entity != null)
            //{
            //    entity.Locationid = id;
            //    context.SaveChanges();
            //}
            //return mapper.ParseUser(entity);
            var existingUser =context.Users.Single(x => x.Id == user.id);
            existingUser.Email = user.email;
            existingUser.Locationid = user.locationId;
            existingUser.Name = user.name;
            existingUser.Type = user.type.ToString();
            context.Entry(existingUser).State = EntityState.Modified;
            context.SaveChanges();
        }
        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns>A list of all users</returns>
        public List<User> GetAllUsers()
        {
            return mapper.ParseUser(context.Users.Select(x => x).ToList());
        }
        /// <summary>
        /// Gets a single user given an id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>Selected user</returns>
        public User GetUserById(int id)
        {
            User user = new User();
            user = mapper.ParseUser(context.Users.Single(x => x.Id == id));
            user.location = GetLocationById(user.locationId);
            user.orders = GetAllOrdersByUserId(user.id);
            user.cart = new Cart();
            return user;
        }
        /// <summary>
        /// Gets a single user given an email
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <returns>Selected user</returns>
        public User GetUserByEmail(string email)
        {
            User user = new User();
            user = mapper.ParseUser(context.Users.Single(x => x.Email == email));
            user.location = GetLocationById(user.locationId);
            user.orders = GetAllOrdersByUserId(user.id);
            user.cart = new Cart();
            return user;
        }
        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="user">User you wish to delete</param>
        public void DeleteUser(int id)
        {
            var entity = context.Users.Single(u => u.Id == id);
            context.Users.Remove(entity);
            context.SaveChanges();
        }
        #endregion
        #region Inventory Item Methods
        public void AddInventoryItem(InventoryItem item)
        {
            context.Inventoryitems.Add(mapper.ParseInventoryItem(item));
            context.SaveChanges();
        }
        public void DeleteInventoryItem(int id)
        {
            var entity = context.Inventoryitems.Single(u => u.Id == id);
            context.Inventoryitems.Remove(entity);
            context.SaveChanges();

        }
        public List<InventoryItem> GetAllInventoryItemsAtLocation(int locId)
        {
            return mapper.ParseInventoryItem(context.Inventoryitems
            .Where(x => x.Locationid == locId)
            .OrderBy(x => x.Id).
            ToList());

        }
        public InventoryItem GetInventoryItem(int locId, int vgId)
        {

            return mapper.ParseInventoryItem(context.Inventoryitems.Single(x => x.Locationid == locId && x.Videogameid == vgId));

        }
        public void UpdateInventoryItem(InventoryItem item)
        {
            var existingItem = context.Inventoryitems.Single(x => x.Id == item.id);
            existingItem.Videogameid = item.videoGameId;
            existingItem.Locationid = item.locationId;
            existingItem.Quantity = item.quantity;
            context.Entry(existingItem).State = EntityState.Modified;
            context.SaveChanges();
        }
        #endregion
        #region Line Item Methods
        public void AddLineItem(LineItem item)
        {
            context.Lineitems.Add(mapper.ParseLineItem(item));
            context.SaveChanges();

        }
        public void UpdateLineItem(LineItem item)
        {
            context.Lineitems.Update(mapper.ParseLineItem(item));
            context.SaveChanges();

        }
        public LineItem GetLineItemByOrderId(int id)
        {

            return mapper.ParseLineItem(context.Lineitems.Single(x => x.Id == id));

        }
        public List<LineItem> GetAllLineItemsByOrderId(int id)
        {
            return mapper.ParseLineItem(context.Lineitems.Where(x => x.Orderid == id).ToList());

        }
        public void DeleteLineItem(LineItem item)
        {
            context.Lineitems.Remove(mapper.ParseLineItem(item));
            context.SaveChanges();

        }
        #endregion
        #region Location Methods
        public void AddLocation(Location location)
        {
            context.Locations.Add(mapper.ParseLocation(location));
            context.SaveChanges();

        }
        public void UpdateLocation(Location location)
        {
            context.Locations.Update(mapper.ParseLocation(location));
            context.SaveChanges();

        }
        public Location GetLocationById(int id)
        {
            return mapper.ParseLocation(context.Locations.Single(x => x.Id == id));

        }
        public List<Location> GetAllLocations()
        {
            return mapper.ParseLocation(context.Locations.Select(x => x).ToList());

        }
        public void DeleteLocation(Location location)
        {
            context.Locations.Remove(mapper.ParseLocation(location));
            context.SaveChanges();

        }
        #endregion
        #region Order Methods
        public void AddOrder(Order order)
        {
            context.Orders.Add(mapper.ParseOrder(order));
            context.SaveChanges();

        }
        public void DeleteOrder(Order order)
        {
            context.Orders.Remove(mapper.ParseOrder(order));
            context.SaveChanges();

        }
        public List<Order> GetAllOrdersByLocationId(int id)
        {
            return mapper.ParseOrder(context.Orders.Where(x => x.Locationid == id).ToList());

        }
        public List<Order> GetAllOrdersByUserId(int id)
        {
            return  mapper.ParseOrder(context.Orders.Where(x => x.Userid == id).ToList());
        }
        public List<Order> GetAllOrdersDateAsc(int userId)
        {
            return mapper.ParseOrder(context.Orders.Where(x => x.Userid == userId).OrderBy(x => x.Orderdate).ToList());

        }
        public List<Order> GetAllOrdersDateDesc(int userId)
        {
            return mapper.ParseOrder(context.Orders.Where(x => x.Userid == userId).OrderByDescending(x => x.Orderdate).ToList());

        }
        public List<Order> GetAllOrdersPriceAsc(int userId)
        {
            return mapper.ParseOrder(context.Orders.Where(x => x.Userid == userId).OrderBy(x => x.Totalcost).ToList());

        }
        public List<Order> GetAllOrdersPriceDesc(int userId)
        {
            return mapper.ParseOrder(context.Orders.Where(x => x.Userid == userId).OrderByDescending(x => x.Totalcost).ToList());

        }
        public List<Order> GetAllLocationOrdersDateAsc(int locationId)
        {
            return mapper.ParseOrder(context.Orders.Where(x => x.Locationid == locationId).OrderBy(x => x.Orderdate).ToList());

        }

        public List<Order> GetAllLocationOrdersDateDesc(int locationId)
        {
            return mapper.ParseOrder(context.Orders.Where(x => x.Locationid == locationId).OrderByDescending(x => x.Orderdate).ToList());

        }

        public List<Order> GetAllLocationOrdersPriceAsc(int locationId)
        {
            return mapper.ParseOrder(context.Orders.Where(x => x.Locationid == locationId).OrderBy(x => x.Totalcost).ToList());

        }

        public List<Order> GetAllLocationOrdersPriceDesc(int locationId)
        {
            return mapper.ParseOrder(context.Orders.Where(x => x.Locationid == locationId).OrderByDescending(x => x.Totalcost).ToList());

        }
        public Order GetOrderByDate(DateTime orderDate)
        {
            return mapper.ParseOrder(context.Orders.Single(x => x.Orderdate == orderDate));

        }
        public Order GetOrderById(int id)
        {
            return mapper.ParseOrder(context.Orders.Single(x => x.Id == id));

        }
        public Order GetOrderByLocationId(int id)
        {
            return mapper.ParseOrder(context.Orders.Single(x => x.Locationid == id));

        }
        public Order GetOrderByUserId(int id)
        {
            return mapper.ParseOrder(context.Orders.Single(x => x.Userid == id));

        }
        public Order UpdateOrderCost(Order order, decimal totalCost)
        {

            var entity = context.Orders.FirstOrDefault(i => i.Id == order.id);
            if (entity != null)
            {
                entity.Totalcost = totalCost;
                context.SaveChanges();
            }
            return mapper.ParseOrder(entity);

        }
        #endregion
        #region Video Game Methods
        public void AddVideoGame(VideoGame videoGame)
        {
            context.Videogames.Add(mapper.ParseVideoGame(videoGame));
            context.SaveChanges();

        }
        public void UpdateVideoGame(VideoGame videoGame)
        {
            context.Update<Videogames>(mapper.ParseVideoGame(videoGame));
            context.SaveChanges();

        }
        public VideoGame GetVideoGame(int id)
        {
            return mapper.ParseVideoGame(context.Videogames.Single(x => x.Id == id));

        }
        public List<VideoGame> GetAllVideoGames()
        {
            return mapper.ParseVideoGame(context.Videogames.Select(x => x).ToList());

        }
        public List<VideoGame> GetAllVideoGamesById(int id)
        {
            return mapper.ParseVideoGame(context.Videogames.Where(x => x.Id == id).ToList());

        }
        public void DeleteVideoGame(VideoGame videoGame)
        {
            context.Videogames.Remove(mapper.ParseVideoGame(videoGame));
            context.SaveChanges();
        }
        #endregion
    }
}