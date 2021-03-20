using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingList.Web.Data;
using ShoppingList.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingList.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly GroceryContext _context;
        private readonly SignInManager<IdentityUser> _sim;
        private readonly UserManager<IdentityUser> _um;
        public bool IsSignedIn { get; set; }
        public IdentityUser currentUser { get; set; }
        [BindProperty] // Works when post
        public InputModel NewInput { get; set; }
        [BindProperty(SupportsGet = true)] // Supports parameter in URL
        public bool KeepOpen { get; set; }

        public IList<GroceryItem> groceryItems { get; set; }

        public IndexModel(GroceryContext context, SignInManager<IdentityUser> sim, UserManager<IdentityUser> um)
        {
            _context = context;
            _sim = sim;
            _um = um;
        }

        public async Task<IActionResult> OnGetAsync(int? changeId)
        {
            IsSignedIn = _sim.IsSignedIn(User);
            if (IsSignedIn)
            {
                currentUser = await _um.GetUserAsync(User);
            }

            groceryItems = await _context.groceryItems.ToListAsync(); // Makes a list (also empty list so that the program doesn't crash)
            if (changeId != null)
            {
                var item = _context.groceryItems.Find(changeId.Value); // Need value due to the fact that it checks if it's not null
                if (item != null && currentUser.Id == item.ByUserID)
                {
                    NewInput = new()
                    {
                        Id = item.ID,
                        ItemDescription = item.ItemDescription,
                        Quantity = item.Quantity
                    };
                }
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            IsSignedIn = _sim.IsSignedIn(User);
            if (IsSignedIn)
            {
                currentUser = await _um.GetUserAsync(User);
            }

            if (NewInput != null)
            {
                if (NewInput.Id == 0)
                {
                    GroceryItem item = new()
                    {
                        ItemDescription = NewInput.ItemDescription,
                        Quantity = NewInput.Quantity,
                        ByUserID = currentUser.Id
                    };
                    _context.groceryItems.Add(item);
                    _context.SaveChanges();
                }
                else
                {
                    GroceryItem item = _context.groceryItems.Find(NewInput.Id);
                    item.ItemDescription = NewInput.ItemDescription;
                    item.Quantity = NewInput.Quantity;

                    _context.SaveChanges();

                }
            }
            if(NewInput.Id == 0)
            {
                return RedirectToPage(new { KeepOpen = true });
            }
            else
            {         
            return RedirectToPage();
            }
        }
        public async Task<IActionResult> OnGetUpdateItemStateAsync(int id)
        {

            if (!_sim.IsSignedIn(User))
            {
                return RedirectToPage("Index");
            }
            var item = await _context.groceryItems.FindAsync(id); // Get the product in the parameter
            if (item != null) // If the program crashes
            {
                item.Purchased = !item.Purchased; // If the box is checked then check out the box and the other way around
                _context.SaveChanges(); // Saves updates to the db
            }
            return RedirectToPage("Index");
        }

        public async Task<IActionResult> OnGetRemoveItemAsync(int id)
        {
            IsSignedIn = _sim.IsSignedIn(User);
            if (IsSignedIn)
            {
                currentUser = await _um.GetUserAsync(User);
            }

            var item = await _context.groceryItems.FindAsync(id);
            if (item != null && IsSignedIn && currentUser.Id == item.ByUserID)
            {
                _context.groceryItems.Remove(item);
                _context.SaveChanges();
            }
            return RedirectToPage("Index");
        }
    }
}

