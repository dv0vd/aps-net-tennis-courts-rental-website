using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace course2.Models
{
    public class Context: DbContext
    {
        public Context() : base("Tennis") { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Coach> Coaches { get; set; }
        public DbSet<Time> Times { get; set; }

    }
}