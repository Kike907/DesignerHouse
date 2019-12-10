using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DesignerHouse;
using DesignerHouse.Data;
using DesignerHouse.Extensions;
using DesignerHouse.Models;
using DesignerHouse.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Deliverables.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController (ApplicationDbContext db)
        {
            _db = db;
        
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                ProductTypes = new List<DesignerHouse.Models.ProductTypes>()
            };
        }

        public IActionResult Index()
        {
            List<int> listShoppingCart = HttpContext.Session.Get<List<int>>("SessionShoppingCart");
            
            if(listShoppingCart.Count > 0)
            {
                foreach(int cartItem in listShoppingCart)
                {
                    ProductTypes prod = _db.ProductTypes.Include(p=>p.SpecialTags).Include(p=>p.Products).Where(p => p.Id == cartItem).FirstOrDefault();
                    ShoppingCartVM.ProductTypes.Add(prod);
                }
            }
            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            //retrieve from session, list of items in cart
            List<int> listCartItems = HttpContext.Session.Get<List<int>>("SessionShoppingCart");

            //add both the time and date to appointment date
            ShoppingCartVM.Appointments.AppointmentDate = ShoppingCartVM.Appointments.AppointmentDate
                                                                .AddHours(ShoppingCartVM.Appointments.AppointmentTime.Hour)
                                                                .AddMinutes(ShoppingCartVM.Appointments.AppointmentTime.Minute);
            //create an object for appointment
           Appointments appointments = ShoppingCartVM.Appointments;
            _db.Appointments.Add(appointments);
            _db.SaveChanges();

            //we can use this id to insert records inside the productSelectedForAppointment
            int appointmentId = appointments.Id;

            foreach(int productTypesId in listCartItems)
            {
                ProductSelectedForAppointment productSelectedForAppointment = new ProductSelectedForAppointment()
                {
                    AppointmentId = appointmentId,
                    ProductTypesId = productTypesId
                };

                _db.ProductSelectedForAppointment.Add(productSelectedForAppointment);
                
            }
            _db.SaveChanges();

            //after we've entered all the new entries inside the db, we need to empty the cart
            listCartItems = new List<int>();
            //we need to set the session again, so it is emptied out.
            HttpContext.Session.Set("SessionShoppingCart", listCartItems);

            return RedirectToAction("AppointmentConfirmation", "ShoppingCart", new {Id = appointmentId});
        }

        public IActionResult Remove(int id)
        {
            List<int> listCartItems = HttpContext.Session.Get<List<int>>("SessionShoppingCart");

            if(listCartItems.Count>0 && listCartItems.Contains(id))
            {
                    listCartItems.Remove(id);
            }

            HttpContext.Session.Set("SessionShoppingCart", listCartItems);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult AppointmentConfirmation(int id)
        {
            //fill the shoppingCartVm based on the appointment Id
            ShoppingCartVM.Appointments = _db.Appointments.Where(a => a.Id == id).FirstOrDefault();

            //we need to retrieve all the products within the appointment to the VM
            //first get the list of products selected for appointment
            List<ProductSelectedForAppointment> objProductList = _db.ProductSelectedForAppointment.Where(p => p.AppointmentId == id).ToList();

            //iterate through the list
            foreach(ProductSelectedForAppointment productAppointmentObj in objProductList)
            {
                //retrieve all products in ProductSelectedForApp and add the shoppingCartVM
                ShoppingCartVM.ProductTypes.Add(_db.ProductTypes.Include(p=>p.Products).Include(p=>p.SpecialTags).Where(p=>p.Id==productAppointmentObj.ProductTypesId).FirstOrDefault());

            }
            return View(ShoppingCartVM);
        }
    }
}