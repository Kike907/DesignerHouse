using System;
using System.Collections.Generic;
using System.Text;
using DesignerHouse.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DesignerHouse.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Products> Products {get; set;}
        public DbSet<SpecialTags> SpecialTags {get; set;}
        public DbSet<ProductTypes> ProductTypes {get; set;}
        public DbSet<Appointments> Appointments {get; set;}
        public DbSet<ProductSelectedForAppointment> ProductSelectedForAppointment {get; set;}
        public DbSet<Orders> Orders {get; set;}
    }
}
