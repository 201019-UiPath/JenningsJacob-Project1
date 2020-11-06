using GGsDB.Repos;
using GGsDB.Models;
using System.Collections.Generic;

namespace GGsLib
{
    public class InventoryItemService
    {
        private IInventoryItemRepo repo;
        public InventoryItemService(IInventoryItemRepo repo)
        {
            this.repo = repo;
        }
        public void AddInventoryItem(InventoryItem item)
        {
            repo.AddInventoryItem(item);
        }
        public void DeleteInventoryItem(InventoryItem item)
        {
            repo.DeleteInventoryItem(item);
        }
        public List<InventoryItem> GetAllInventoryItemById(int id)
        {
            return repo.GetAllInventoryItemById(id);
        }
        public List<InventoryItem> GetAllInventoryItemByLocationId(int id)
        {
            return repo.GetAllInventoryItemByLocationId(id);
        }
        public InventoryItem GetInventoryItemById(int id)
        {
            return repo.GetInventoryItemById(id);
        }
        public InventoryItem GetInventoryItemByLocationId(int id)
        {
            return repo.GetInventoryItemByLocationId(id);
        }
        public InventoryItem GetInventoryItem(int locId, int vgId)
        {
            return repo.GetInventoryItem(locId, vgId);
        }
        public void DiminishInventoryItem(InventoryItem item, int quantity)
        {
            repo.DiminishInventoryItem(item, quantity);
        }
        public void ReplenishInventoryItem(InventoryItem item, int quantity)
        {
            repo.ReplenishInventoryItem(item, quantity);
        }
    }
}