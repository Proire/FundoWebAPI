using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserRLL.Entity;

namespace UserRLL.Context
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options): base(options)
        {
        }

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<NoteEntity> Notes { get; set; }

        public DbSet<LabelEntity> Labels { get; set; }

        public DbSet<NoteLabelEntity> NoteLabels { get; set; }

        public DbSet<CollaboraterEntity> Collaboraters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.UserName)
                .IsUnique();

            modelBuilder.Entity<UserEntity>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();

            modelBuilder.Entity<NoteEntity>()
            .HasOne(n => n.UserEntity) // NoteEntity has one UserEntity
            .WithMany(u => u.Notes) // UserEntity can have many NoteEntity instances
            .HasForeignKey(n => n.UserEntityId); // Foreign key property in NoteEntity

            modelBuilder.Entity<CollaboraterEntity>().
                HasOne(n => n.NoteEntity) 
                .WithMany(n => n.CollaboraterEntities)
                .HasForeignKey(n => n.NoteEntityId);

            // for Many to Many relationship between Note and Label 
            modelBuilder.Entity<NoteLabelEntity>()
           .HasKey(nl => new { nl.NoteId, nl.LabelId });

            modelBuilder.Entity<NoteLabelEntity>()
                .HasOne(nl => nl.Note)
                .WithMany(n => n.NoteLabels)
                .HasForeignKey(nl => nl.NoteId);

            modelBuilder.Entity<NoteLabelEntity>()
                .HasOne(nl => nl.Label)
                .WithMany(l => l.NoteLabels)
                .HasForeignKey(nl => nl.LabelId);
        }
    }
}
