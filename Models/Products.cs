using System.ComponentModel.DataAnnotations;

namespace DesignerHouse.Models
{
    public class Products
    {
        public int Id {get; set;}

        [Required]
        public string Name {get; set;}
    }
}