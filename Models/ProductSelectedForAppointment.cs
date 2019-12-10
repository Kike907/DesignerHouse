using System.ComponentModel.DataAnnotations.Schema;
using DesignerHouse.Models;

namespace DesignerHouse.Models
{
    public class ProductSelectedForAppointment
    {
        public int Id {get; set;}

        public int AppointmentId { get; set; }

        [ForeignKey("AppointmentId")]
        public virtual Appointments Appointments { get; set; }

        public int ProductTypesId { get; set; }

        [ForeignKey("ProductId")]
        public virtual ProductTypes ProductTypes { get; set; }

    }
}