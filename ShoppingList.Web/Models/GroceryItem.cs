using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Web.Models
{
    public class GroceryItem
    {
        public int ID { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int Quantity { get; set; }
        public bool Purchased { get; set; }
        public string ByUserID { get; set; } // User who posted the item

    }
}
