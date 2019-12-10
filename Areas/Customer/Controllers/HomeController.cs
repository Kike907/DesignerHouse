using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DesignerHouse.Models;
using DesignerHouse.Data;
using Microsoft.EntityFrameworkCore;
using DesignerHouse.Extensions;
using Microsoft.AspNetCore.Http;
namespace DesignerHouse.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            //retrieve all products from db
            var productList = await _db.ProductTypes.Include(m => m.Products).Include(m => m.SpecialTags).ToListAsync();
            
            return View(productList);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.ProductTypes.Include(m => m.Products).Include(m => m.SpecialTags).Where(m=> m.Id == id).FirstOrDefaultAsync();
            
            return View(product );
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public IActionResult DetailsPost(int id)
        {
            //First check if anything exists in the session
            List<int> listShoppingCart = HttpContext.Session.Get<List<int>>("SessionShoppingCart");
            if (listShoppingCart == null)
            {
                listShoppingCart = new List<int>();
            }
            listShoppingCart.Add(id);
            HttpContext.Session.Set("SessionShoppingCart", listShoppingCart); //set session variable(SessionShoppingCart to the value of the listShoppingCart)

            return RedirectToAction("Index", "Home", new {area="Customer"});
        }

        public IActionResult Remove(int id)
        {
            List<int> listShoppingCart = HttpContext.Session.Get<List<int>>("SessionShoppingCart");
            if(listShoppingCart.Count>0)
            {
                if (listShoppingCart.Contains(id))
                {
                    listShoppingCart.Remove(id);
                }
            }

            HttpContext.Session.Set("SessionShoppingCart", listShoppingCart);

            return RedirectToAction(nameof(Index));
        }
    }
}
