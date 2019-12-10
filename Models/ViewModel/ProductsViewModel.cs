using System.Collections.Generic;

namespace DesignerHouse.Models.ViewModel
{
    public class ProductsViewModel
    {
        public ProductTypes ProductTypes {get; set;}

        public IEnumerable<Products> Products {get; set;}

        public IEnumerable<SpecialTags> SpecialTags {get; set;}
    }
}