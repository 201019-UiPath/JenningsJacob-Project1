using GGsDB.Models;
using System.Collections.Generic;

namespace GGsLib
{
    public interface ILineItemService
    {
        void AddLineItem(LineItem item);
        void DeleteLineItem(LineItem item);
        List<LineItem> GetAllLineItemsById(int id);
        LineItem GetLineItemById(int id);
        void UpdateLineItem(LineItem item);
    }
}