using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesignerHouse.Models
{
    public class Orders
  {
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    [NotMapped]
    public DateTime OrderTime { get; set; }

    public string CustomerName { get; set; }

    public string CustomerPhoneNumber { get; set; }

    public string CustomerEmail { get; set; }

    public bool isConfirmed { get; set; }
  }
}