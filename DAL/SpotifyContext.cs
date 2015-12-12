using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using SpotifyDataClient.Models;

namespace SpotifyDataClient.DAL
{
    public class SpotifyContext : DbContext
    {
        public SpotifyContext() : base("SpotifyContext")
        {
        }

        public DbSet<Artist> Artists { get; set; } 
        public DbSet<Album> Albums { get; set; }
        public DbSet<Song> Songs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}