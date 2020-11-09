using System.Collections.Generic;
using GGsDB.Models;

namespace GGsDB.Repos
{
    public interface IInventoryItemRepo
{
        void AddInventoryItem(InventoryItem item);
        void DeleteInventoryItem(int id);
        List<InventoryItem> GetAllInventoryItemsAtLocation(int locationId);
        InventoryItem GetInventoryItem(int locationId, int videoGameId);
        void UpdateInventoryItem(InventoryItem item);

    }
}