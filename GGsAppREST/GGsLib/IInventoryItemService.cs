using GGsDB.Models;
using System.Collections.Generic;

namespace GGsLib
{
    public interface IInventoryItemService
    {
        void AddInventoryItem(InventoryItem item);
        void DeleteInventoryItem(InventoryItem item);
        void DiminishInventoryItem(InventoryItem item, int quantity);
        List<InventoryItem> GetAllInventoryItemById(int id);
        List<InventoryItem> GetAllInventoryItemByLocationId(int id);
        InventoryItem GetInventoryItem(int locId, int vgId);
        InventoryItem GetInventoryItemById(int id);
        InventoryItem GetInventoryItemByLocationId(int id);
        void ReplenishInventoryItem(InventoryItem item, int quantity);
    }
}