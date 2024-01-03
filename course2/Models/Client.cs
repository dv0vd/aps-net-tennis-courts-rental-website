using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace course2.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public List<Purchase> Purchases { get; set; }
    }
}