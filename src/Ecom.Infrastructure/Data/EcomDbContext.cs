using Ecom.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data
{
    public class EcomDbContext: DbContext
    {
        public EcomDbContext(DbContextOptions<EcomDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // من خلال هاد السطر رح يبدأ يدور على ال Configurations من أي مكان بيورث من ال IEntityTypeConfiguration ويبدأ ينفذ ال validations يلي بقلبو
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
