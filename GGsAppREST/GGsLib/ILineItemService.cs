using GGsDB.Models;
using System.Collections.Generic;

namespace GGsLib
{
    public interface ILineItemService
    {
        void AddLineItem(LineItem item);
        void DeleteLineItem(LineItem item);
        List<LineItem> GetAllLineItems(int orderId);
        LineItem GetLineItemById(int id);
        void UpdateLineItem(LineItem item);
    }
}