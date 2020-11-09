using System.Collections.Generic;
using GGsDB.Models;

namespace GGsDB.Repos
{
    public interface IInventoryItemRepo
{
        void AddInventoryItem(InventoryItem item);
        void DiminishInventoryItem(InventoryItem item, int quantity);
        void ReplenishInventoryItem(InventoryItem item, int quantity);
        InventoryItem GetInventoryItemById(int id);
        List<InventoryItem> GetAllInventoryItemById(int id);
        InventoryItem GetInventoryItemByLocationId(int id);
        List<InventoryItem> GetAllInventoryItemByLocationId(int id);
        InventoryItem GetInventoryItem(int locId, int vgId);
        void DeleteInventoryItem(InventoryItem item);

    }
}