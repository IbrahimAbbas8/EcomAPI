using Ecom.Core.Entities;
using Ecom.Core.Entities.Orders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data
{
    public class EcomDbContext: IdentityDbContext<AppUser>
    {
        public EcomDbContext(DbContextOptions<EcomDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // من خلال هاد السطر رح يبدأ يدور على ال Configurations من أي مكان بيورث من ال IEntityTypeConfiguration ويبدأ ينفذ ال validations يلي بقلبو
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
