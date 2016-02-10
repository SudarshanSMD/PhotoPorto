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

            //many-to-many relation of Gallery and Photogrph tables using GalleryPhotograph table
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
