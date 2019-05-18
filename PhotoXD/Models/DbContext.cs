using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace PhotoXD.Models
{
    public class Guestbookcontext : DbContext
    {
        public Guestbookcontext()
            : base("GuestbookEntries")
        {
        }
        public DbSet<GuestbookEntry> Entries { get; set; }
    }
}