﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GGsWeb.Models
{
    public class VideoGame
    {
        public int id { get; set; }
        public string name { get; set; }
        [DataType(DataType.Currency)]
        public decimal cost { get; set; }
        [DisplayName("Platform")]
        public string platform { get; set; }
        [DisplayName("Rating")]
        public string esrb { get; set; }
        public string description { get; set; }
    }
}