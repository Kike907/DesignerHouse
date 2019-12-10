using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignerHouse.Models
{
    public class ProductTypes
    {
        public int Id {get; set;}
        public string Name {get; set;}
        public double Price {get; set;}
        public bool Available {get; set;}
        public string Image {get; set;}
        public string ShadeColor {get; set;}

        [Display(Name="Products")]
        public int ProductId {get; set;}

        [ForeignKey("ProductId")]
        public virtual Products Products {get; set;}

        [Display(Name="Special Tags")]
        public int SpecialTagsId {get; set;}

        [ForeignKey("SpecialTagsId")]
        public virtual SpecialTags SpecialTags {get; set;}
    }
}