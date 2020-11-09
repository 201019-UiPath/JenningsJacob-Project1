using System.Collections.Generic;
using System.ComponentModel;
using System;

namespace GGsDB.Models
{
    public class VideoGame
    {
        public int id {get; set;}
        public string name {get; set;}
        public decimal cost {get; set;}
        public string platform {get; set;}

        public string esrb {get; set;}
        public void PrintInfo()
        {
            Console.WriteLine($"{name}\t${cost}\t{platform}\t{esrb}");
        }
    }
}