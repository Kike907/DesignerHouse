using System.Threading.Tasks;
using DesignerHouse.Data;
using DesignerHouse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesignerHouse.Areas
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            return View (await _db.Products.ToListAsync());
        }

        public IActionResult Create()
        {
             return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Create(Products products)
         {
             if(ModelState.IsValid)
             {
                   _db.Add(products);
                   await _db.SaveChangesAsync();
                   return RedirectToAction(nameof(Index));
             }
             return View(products); 
         }  

        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var products = await _db.Products.FindAsync(id);
            if(products == null)
            {
                return NotFound();
            }
             return View(products);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> Edit(int id, Products products)
         {
             if (id != products.Id)
             {
                 return NotFound();
             }
             if(ModelState.IsValid)
             {
                   _db.Update(products);
                   await _db.SaveChangesAsync();
                   return RedirectToAction(nameof(Index));
             }
             return View(products); 
         }  

        public async Task<IActionResult> Details(int? id)
         {
             if (id == null)
             {
                 return NotFound();
             }
             var products = await _db.Products.FindAsync(id);

             if (products == null)
             {
                 return NotFound();
             }
             return View(products);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var products = await _db.Products.FindAsync(id);
            if(products == null)
            {
                return NotFound();
            }
             return View(products);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
         public async Task<IActionResult> DeleteConfirmed(int id)
         {
             var products = await _db.Products.FindAsync(id);
             _db.Products.Remove(products);
             await _db.SaveChangesAsync();
             return RedirectToAction(nameof(Index));
             
         }  
        
    }
}