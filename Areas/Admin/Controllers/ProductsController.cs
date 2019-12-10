using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DesignerHouse.Data;
using DesignerHouse.Models;
using DesignerHouse.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using DesignerHouse.Utility;
using System.IO;

namespace DesignerHouse.Areas
{
    [Area("Admin")]
    public class ProductsController : Controller
    { 
        private readonly ApplicationDbContext _context;
         private readonly IHostingEnvironment _hostingEnvironment;

        [BindProperty]
        public ProductsViewModel ProductsVM {get; set; }

        public ProductsController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            ProductsVM = new ProductsViewModel()
            {
                Products = _context.Products.ToList(),
                SpecialTags = _context.SpecialTags.ToList(),
                ProductTypes = new Models.ProductTypes()
            };
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = _context.ProductTypes.Include(p => p.Products).Include(s => s.SpecialTags);
            return View(await _context.ProductTypes.ToListAsync());
        }

        public IActionResult Create()
        {
            return View(ProductsVM);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePOST()
        {
            #region Add products
            if (!ModelState.IsValid)
            { 
                return View(ProductsVM);
               
            }
             _context.ProductTypes.Add(ProductsVM.ProductTypes);
                await _context.SaveChangesAsync();
            #endregion

            #region Retrieve from database
                //Image being saved
                string webRootPath = _hostingEnvironment.WebRootPath; //retrieve the root path of the app
                var files = HttpContext.Request.Form.Files; //retrieve all files
                
                var productFromDb = _context.ProductTypes.Find(ProductsVM.ProductTypes.Products.Id);
            #endregion
                
            #region Change file Name to product Id  
                //files contains all files uploaded from the view
                if (files.Count!=0)
                {
                    //Image has been uploaded
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder); //find path for upload
                    var extension = Path.GetExtension(files[0].FileName); //get the extension of the file
                
                    //use filestream object to copy the file from the upload to the server
                      using(var filestream = new FileStream(Path.Combine(uploads, ProductsVM.ProductTypes.Id + extension), FileMode.Create))//this would create file on the server
                      {
                          files[0].CopyTo(filestream); //this would move the file to the server and then rename it
                      }

                    productFromDb.Image = @"\"+SD.ImageFolder + @"\"+ ProductsVM.ProductTypes.Id + extension ;
                }
                else
                {
                    //when user does not upload image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                    //copy image from the server and rename, so that the default image would have the product ID
                    System.IO.File.Copy(uploads, webRootPath+@"\"+SD.ImageFolder+@"\"+ProductsVM.ProductTypes.Id +".jpg");
                     productFromDb.Image = @"\"+SD.ImageFolder + @"\"+ ProductsVM.ProductTypes.Id + ".jpg" ;
                    
                 }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }
            #endregion

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

             ProductsVM.ProductTypes = await _context.ProductTypes.Include(s => s.SpecialTags).Include(p => p.Products).SingleOrDefaultAsync(i => i.Id == id);

            if(ProductsVM.ProductTypes == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            if(ModelState.IsValid)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;

                var productFromDb = _context.ProductTypes.Where(m => m.Id == ProductsVM.ProductTypes.Id).FirstOrDefault();

                if(files.Count > 0 && files[0] != null)
                {
                    //if user uploads a new image
                    var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                    var extension_new = Path.GetExtension(files[0].FileName);
                    var extension_old = Path.GetExtension(productFromDb.Image);

                    if(System.IO.File.Exists(Path.Combine(uploads, ProductsVM.ProductTypes.Id+extension_old)))
                    {
                        System.IO.File.Delete(Path.Combine(uploads, ProductsVM.ProductTypes.Id + extension_old));
                    }

                    using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.ProductTypes.Id + extension_new),FileMode.Create))
                    {
                        files[0].CopyTo(filestream);
                    }

                    ProductsVM.ProductTypes.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.ProductTypes.Id + extension_new;

                }

                if (ProductsVM.ProductTypes.Image != null)
                {
                    productFromDb.Image = ProductsVM.ProductTypes.Image;
                }

                productFromDb.Name = ProductsVM.ProductTypes.Name;
                productFromDb.Price = ProductsVM.ProductTypes.Price;
                productFromDb.Available = ProductsVM.ProductTypes.Available;
                productFromDb.ProductId = ProductsVM.ProductTypes.ProductId;
                productFromDb.SpecialTagsId = ProductsVM.ProductTypes.SpecialTagsId;
                productFromDb.ShadeColor = ProductsVM.ProductTypes.ShadeColor;
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }

            return View(ProductsVM);

        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

             ProductsVM.ProductTypes = await _context.ProductTypes.Include(s => s.SpecialTags).Include(p => p.Products).SingleOrDefaultAsync(i => i.Id == id);

            if(ProductsVM.ProductTypes == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

             ProductsVM.ProductTypes = await _context.ProductTypes.Include(s => s.SpecialTags).Include(p => p.Products).SingleOrDefaultAsync(i => i.Id == id);

            if(ProductsVM.ProductTypes == null)
            {
                return NotFound();
            }

            return View(ProductsVM);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string webRootPath = _hostingEnvironment.WebRootPath;
            ProductTypes products = await _context.ProductTypes.FindAsync(id);
           // var productTypes = await _context.Products.FindAsync(id);

            if(products == null)
            {
                return NotFound();
            }
            else{
                var uploads = Path.Combine(webRootPath, SD.ImageFolder);
                var extension = Path.GetExtension(products.Image);

                if (System.IO.File.Exists(Path.Combine(uploads, products.Id+extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, products.Id+extension));
                }

                _context.ProductTypes.Remove(products);
            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index));
            }
        }

        private bool ProductsExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        } 

        
    }
}
