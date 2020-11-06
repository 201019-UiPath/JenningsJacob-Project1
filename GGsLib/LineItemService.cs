using GGsDB.Models;
using GGsDB.Repos;
using System.Collections.Generic;

namespace GGsLib
{
    public class LineItemService
    {
        private ILineItemRepo repo;
        public LineItemService(ILineItemRepo repo)
        {
            this.repo = repo;
        }
        public void AddLineItem(LineItem item)
        {
            repo.AddLineItem(item);
        }
        public void DeleteLineItem(LineItem item)
        {
            repo.DeleteLineItem(item);
        }
        public List<LineItem> GetAllLineItemsById(int id)
        {
            return repo.GetAllLineItemsByOrderId(id);
        }
        public LineItem GetLineItemById(int id)
        {
            return repo.GetLineItemByOrderId(id);
        }
        public void UpdateLineItem(LineItem item)
        {
            repo.UpdateLineItem(item);
        }
    }
}