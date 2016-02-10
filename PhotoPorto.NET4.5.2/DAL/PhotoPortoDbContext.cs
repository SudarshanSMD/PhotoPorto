using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using PhotoPorto.Models;

using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PhotoPorto.DAL
{
    //public class ApplicationDbContext : DbContext

    public class PhotoPortoDbContext : DbContext
    {
     
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            /*
         modelBuilder.Entity<Gallery>()
         .HasMany(c => c.Photographs).WithMany(i => i.Galleries)
         .Map(t => t.MapLeftKey("GalleryID")
             .MapRightKey("PhotographsID")
             .ToTable("GalleryPhotograph"));
             */

            modelBuilder.Entity<Gallery>()
              .HasMany<Photograph>(s => s.Photographs)
              .WithMany(c => c.Galleries)
              .Map(cs =>
              {
                  cs.MapLeftKey("GalleryID");
                  cs.MapRightKey("PhotographsID");
                  cs.ToTable("GalleryPhotograph");
              });


        }
        public DbSet<Gallery> Gallery { get; set; }
		public DbSet<Photograph> Photograph { get; set; }
    }

       
}
