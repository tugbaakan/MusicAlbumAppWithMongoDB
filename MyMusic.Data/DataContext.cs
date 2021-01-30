using Microsoft.EntityFrameworkCore;
using MyMusic.Core.Models;
using MyMusic.Data.Configurations;
using System;

namespace MyMusic.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }        
        public DbSet<Music> Musics { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Music>()
                .HasKey(m => m.Id);

            builder.Entity<Music>()
                .Property(m => m.Id)
                .UseIdentityColumn();

            builder.Entity<Music>()
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            /*builder.Entity<Music>()
                .HasOne(m => m.Artist)
                .WithMany(a => a.Musics)
                .HasForeignKey(m => m.ArtistId);

            builder.Entity<Music>()
                .ToTable("Musics"); */


            builder.Entity<Artist>()
               .HasKey(a => a.Id);

            builder.Entity<Artist>()
                .Property(m => m.Id)
                .UseIdentityColumn();

            builder.Entity<Artist>()
                .Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(50);

            /*builder.Entity<Artist>()
                .ToTable("Artists");*/


            builder.Entity<User>()
                .HasKey(m => m.Id);

            builder.Entity<User>()
                .Property(m => m.Id)
                .UseIdentityColumn();

            builder.Entity<User>()
                .Property(m => m.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Entity<User>()
             .Property(m => m.Username)
             .IsRequired()
             .HasMaxLength(50);

            builder.Entity<User>()
                  .Property(m => m.LastName)
                 .IsRequired()
                 .HasMaxLength(50);
            /*builder
                .ToTable("Users");*/

        }
    }
}
