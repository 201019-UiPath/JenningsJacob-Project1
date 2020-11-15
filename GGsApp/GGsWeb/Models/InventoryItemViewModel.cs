using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GGsWeb.Models
{
    public class InventoryItemViewModel
    {
        public string name { get; set; }
        public string platform { get; set; }
        public string esrb { get; set; }
        public int quantity { get; set; }
        public decimal cost { get; set; }
        public string description { get; set; }
    }
}
