using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Repos
{
    public interface ILineItemRepo
    {
        void AddLineItem(LineItem item);
        void UpdateLineItem(LineItem item);
        LineItem GetLineItemByOrderId(int id);
        List<LineItem> GetAllLineItems(int orderId);
        void DeleteLineItem(LineItem item);
    }
}