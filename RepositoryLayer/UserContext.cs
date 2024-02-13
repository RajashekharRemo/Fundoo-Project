using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {

        }

        public DbSet<User> OnlineUser2 { get; set; }
        //public DbSet<Notes> Notes { get; set; } // u can use add-migration only 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e=>e.First_Name).IsRequired().HasMaxLength(150);
                entity.Property(e=>e.Last_Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Password).IsRequired().HasMaxLength(250);
            });
        }

    }
}
