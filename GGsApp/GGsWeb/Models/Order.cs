﻿using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GGsWeb.Models
{
    public class Order
    {
        public int id { get; set; }
        public int userId { get; set; }
        public User user { get; set; }
        public int locationId { get; set; }
        public Location location { get; set; }
        public DateTime orderDate { get; set; }
        [DataType(DataType.Currency)]
        public decimal totalCost { get; set; }
        public List<LineItem> lineItems { get; set; }
    }
}
