using System.Collections.Generic;
using GGsDB.Models;

namespace GGsDB.Repos
{
    public interface ILineItemRepo
    {
        void AddLineItem(LineItem item);
        void UpdateLineItem(LineItem item);
        LineItem GetLineItemByOrderId(int id);
        List<LineItem> GetAllLineItemsByOrderId(int id);
        void DeleteLineItem(LineItem item);
    }
}