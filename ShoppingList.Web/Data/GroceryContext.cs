using Microsoft.EntityFrameworkCore;
using ShoppingList.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Web.Data
{
    public class GroceryContext : DbContext
    {
        public GroceryContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<GroceryItem> groceryItems { get; set; }

    }
}
