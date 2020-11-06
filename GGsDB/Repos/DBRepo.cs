using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GGsDB.Entities;
using GGsDB.Mappers;
using GGsDB.Models;

namespace GGsDB.Repos
{
    /// <summary>
    /// Repository responsible for interfacing directlly with the database
    /// </summary>
    public class DBRepo : IRepo
    {
        private GGsContext context1;
        private DBMapper mapper;
        public DBRepo(GGsContext context1, DBMapper mapper)
        {
            this.context1 = context1;
            this.mapper = mapper;
        }
        #region User Methods
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user">User you wish to add</param>
        public void AddUser(User user)
        {
            //using (GGsContext context = new GGsContext())
            //{
                context1.Users.Add(mapper.ParseUser(user));
                context1.SaveChanges();
            //}
        }
        /// <summary>
        /// Updates a user in the database
        /// </summary>
        /// <param name="user">Updated user</param>
        public User UpdateUserLocationId(User user, int id)
        {
            using (GGsContext context = new GGsContext())
            {
                var entity = context.Users.FirstOrDefault(i => i.Id == user.id);
                if (entity != null)
                {
                    entity.Locationid = id;
                    context.SaveChanges();
                }
                return mapper.ParseUser(entity);
            }
        }
        /// <summary>
        /// Gets all users from the database
        /// </summary>
        /// <returns>A list of all users</returns>
        public List<User> GetAllUsers()
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseUser(context.Users.Select(x => x).ToList());
            }
        }
        /// <summary>
        /// Gets a single user given an id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>Selected user</returns>
        public User GetUserById(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                User user = new User();
                user = mapper.ParseUser(context.Users.Single(x => x.Id == id));
                user.location = GetLocationById(user.locationId);
                return user;
            }
        }
        /// <summary>
        /// Gets a single user given an email
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <returns>Selected user</returns>
        public User GetUserByEmail(string email)
        {
            using (GGsContext context = new GGsContext())
            {
                User user = new User();
                user = mapper.ParseUser(context.Users.Single(x => x.Email == email));
                user.location = GetLocationById(user.locationId);
                return user;
            }
        }
        /// <summary>
        /// Deletes a user from the database
        /// </summary>
        /// <param name="user">User you wish to delete</param>
        public void DeleteUser(User user)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Users.Remove(mapper.ParseUser(user));
                context.SaveChanges();
            }
        }
        #endregion
        
        #region Inventory Item Methods
        public void AddInventoryItem(InventoryItem item)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Inventoryitems.Add(mapper.ParseInventoryItem(item));
                context.SaveChanges();
            }
        }
        public void ReplenishInventoryItem(InventoryItem item, int quantity)
        {
            using (GGsContext context = new GGsContext())
            {
                var entity = context.Inventoryitems.FirstOrDefault(i => i.Id == item.id);
                if (entity != null)
                {
                    entity.Quantity += quantity;
                    // context.Inventoryitems.Update(mapper.ParseInventoryItem(item));
                    context.SaveChanges();
                }
            }
        }
        public void DiminishInventoryItem(InventoryItem item, int quantity)
        {
            using (GGsContext context = new GGsContext())
            {
                var entity = context.Inventoryitems.FirstOrDefault(i => i.Id == item.id);
                if (entity != null)
                {
                    entity.Quantity -= quantity;
                    // context.Inventoryitems.Update(mapper.ParseInventoryItem(item));
                    context.SaveChanges();
                }
            }
        }
        public InventoryItem GetInventoryItemById(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseInventoryItem(context.Inventoryitems.Single(x => x.Id == id));
            }
        }
        public List<InventoryItem> GetAllInventoryItemById(int id)
        {   
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseInventoryItem(context.Inventoryitems
                .Where(x => x.Id == id)
                .OrderBy(x => x.Id)
                .ToList());
            }
        }
        public InventoryItem GetInventoryItemByLocationId(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseInventoryItem(context.Inventoryitems.Single(x => x.Locationid == id));
            }
        }
        public List<InventoryItem> GetAllInventoryItemByLocationId(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseInventoryItem(context.Inventoryitems
                .Where(x => x.Locationid == id)
                .OrderBy(x => x.Id).
                ToList());
            }
        }
        public InventoryItem GetInventoryItem(int locId, int vgId)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseInventoryItem(context.Inventoryitems.Single(x => x.Locationid == locId && x.Videogameid == vgId));
            }
        }
        public void DeleteInventoryItem(InventoryItem item)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Inventoryitems.Remove(mapper.ParseInventoryItem(item));
                context.SaveChanges();
            }
        }
        #endregion
        #region Line Item Methods
        public void AddLineItem(LineItem item)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Lineitems.Add(mapper.ParseLineItem(item));
                context.SaveChanges();
            }
        }
        public void UpdateLineItem(LineItem item)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Lineitems.Update(mapper.ParseLineItem(item));
                context.SaveChanges();
            }
        }
        public LineItem GetLineItemByOrderId(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseLineItem(context.Lineitems.Single(x => x.Id == id));
            }
        }
        public List<LineItem> GetAllLineItemsByOrderId(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseLineItem(context.Lineitems.Where(x => x.Orderid == id).ToList());
            }
        }
        public void DeleteLineItem(LineItem item)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Lineitems.Remove(mapper.ParseLineItem(item));
                context.SaveChanges();
            }
        }
        #endregion
        #region Location Methods
        public void AddLocation(Location location)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Locations.Add(mapper.ParseLocation(location));
                context.SaveChanges();
            }
        }
        public void UpdateLocation(Location location)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Locations.Update(mapper.ParseLocation(location));
                context.SaveChanges();
            }
        }
        public Location GetLocationById(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseLocation(context.Locations.Single(x => x.Id == id));
            }
        }
        public List<Location> GetAllLocations()
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseLocation(context.Locations.Select(x => x).ToList());
            }
        }
        public void DeleteLocation(Location location)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Locations.Remove(mapper.ParseLocation(location));
                context.SaveChanges();
            }
        }
        #endregion
        #region Order Methods
        public void AddOrder(Order order)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Orders.Add(mapper.ParseOrder(order));
                context.SaveChanges();
            }
        }
        public void DeleteOrder(Order order)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Orders.Remove(mapper.ParseOrder(order));
                context.SaveChanges();
            }
        }
        public List<Order> GetAllOrdersByLocationId(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Where(x => x.Locationid == id).ToList());
            }
        }
        public List<Order> GetAllOrdersByUserId(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Where(x => x.Userid == id).ToList());
            }
        }
        public List<Order> GetAllOrdersDateAsc(int userId)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Where(x => x.Userid == userId).OrderBy(x => x.Orderdate).ToList());
            }
        }
        public List<Order> GetAllOrdersDateDesc(int userId)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Where(x => x.Userid == userId).OrderByDescending(x => x.Orderdate).ToList());
            }
        }
        public List<Order> GetAllOrdersPriceAsc(int userId)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Where(x => x.Userid == userId).OrderBy(x => x.Totalcost).ToList());
            }
        }
        public List<Order> GetAllOrdersPriceDesc(int userId)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Where(x => x.Userid == userId).OrderByDescending(x => x.Totalcost).ToList());
            }
        }
        public List<Order> GetAllLocationOrdersDateAsc(int locationId)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Where(x => x.Locationid == locationId).OrderBy(x => x.Orderdate).ToList());
            }
        }

        public List<Order> GetAllLocationOrdersDateDesc(int locationId)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Where(x => x.Locationid == locationId).OrderByDescending(x => x.Orderdate).ToList());
            }
        }

        public List<Order> GetAllLocationOrdersPriceAsc(int locationId)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Where(x => x.Locationid == locationId).OrderBy(x => x.Totalcost).ToList());
            }
        }

        public List<Order> GetAllLocationOrdersPriceDesc(int locationId)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Where(x => x.Locationid == locationId).OrderByDescending(x => x.Totalcost).ToList());
            }
        }
        public Order GetOrderByDate(DateTime orderDate)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Single(x => x.Orderdate == orderDate));
            }
        }
        public Order GetOrderById(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Single(x => x.Id == id));
            }
        }
        public Order GetOrderByLocationId(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Single(x => x.Locationid == id));
            }
        }
        public Order GetOrderByUserId(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseOrder(context.Orders.Single(x => x.Userid == id));
            }
        }
        public Order UpdateOrderCost(Order order, decimal totalCost)
        {
            using (GGsContext context = new GGsContext())
            {
                var entity = context.Orders.FirstOrDefault(i => i.Id == order.id);
                if (entity != null)
                {
                    entity.Totalcost = totalCost;
                    context.SaveChanges();
                }
                return mapper.ParseOrder(entity);
            }
        }
        #endregion
        #region Video Game Methods
        public void AddVideoGame(VideoGame videoGame)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Videogames.Add(mapper.ParseVideoGame(videoGame));
                context.SaveChanges();
            }
        }
        public void UpdateVideoGame(VideoGame videoGame)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Update<Videogames>(mapper.ParseVideoGame(videoGame));
                context.SaveChanges();
            }
        }
        public VideoGame GetVideoGame(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseVideoGame(context.Videogames.Single(x => x.Id == id));
            }
        }
        public List<VideoGame> GetAllVideoGames()
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseVideoGame(context.Videogames.Select(x => x).ToList());
            }
        }
        public List<VideoGame> GetAllVideoGamesById(int id)
        {
            using (GGsContext context = new GGsContext())
            {
                return mapper.ParseVideoGame(context.Videogames.Where(x => x.Id == id).ToList());
            }
        }
        public void DeleteVideoGame(VideoGame videoGame)
        {
            using (GGsContext context = new GGsContext())
            {
                context.Videogames.Remove(mapper.ParseVideoGame(videoGame));
                context.SaveChanges();
            }
        }
        #endregion
    }
} 