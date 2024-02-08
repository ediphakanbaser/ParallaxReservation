using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Parallax.Models
{
    public partial class ParallaxContext : DbContext
    {
        public ParallaxContext()
            : base("name=ParallaxContext1")
        {
        }

        public virtual DbSet<ROLE> ROLES { get; set; }
        public virtual DbSet<SKILL> SKILLS { get; set; }
        public virtual DbSet<TBLEMPLOYEE> TBLEMPLOYEEs { get; set; }
        public virtual DbSet<TBLRESERVATION> TBLRESERVATIONs { get; set; }
        public virtual DbSet<TBLREVIEW> TBLREVIEWs { get; set; }
        public virtual DbSet<TBLSERVICE> TBLSERVICEs { get; set; }
        public virtual DbSet<TBLUSER> TBLUSERs { get; set; }
        public virtual DbSet<TYPE> TYPES { get; set; }
        public virtual DbSet<TBLPAGE> TBLPAGEs { get; set; }
        public virtual DbSet<EmpService> EmpServices { get; set; }
        public virtual DbSet<PaketHizmet> PaketHizmets { get; set; }
        public virtual DbSet<ReservationInfo> ReservationInfoes { get; set; }
        public virtual DbSet<ServiceType> ServiceTypes { get; set; }
        public virtual DbSet<TekilHizmetler> TekilHizmetlers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ROLE>()
                .HasMany(e => e.TBLUSERs)
                .WithRequired(e => e.ROLE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SKILL>()
                .HasMany(e => e.TBLRESERVATIONs)
                .WithRequired(e => e.SKILL)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TBLEMPLOYEE>()
                .Property(e => e.EmpSalary)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TBLEMPLOYEE>()
                .HasMany(e => e.SKILLS)
                .WithRequired(e => e.TBLEMPLOYEE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TBLREVIEW>()
                .Property(e => e.Comment)
                .IsUnicode(false);

            modelBuilder.Entity<TBLSERVICE>()
                .Property(e => e.ServicePrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TBLSERVICE>()
                .Property(e => e.DiscountedPrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<TBLSERVICE>()
                .Property(e => e.TimeSpent)
                .HasPrecision(0);

            modelBuilder.Entity<TBLSERVICE>()
                .HasMany(e => e.SKILLS)
                .WithRequired(e => e.TBLSERVICE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TBLUSER>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<TBLUSER>()
                .HasMany(e => e.TBLRESERVATIONs)
                .WithRequired(e => e.TBLUSER)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TYPE>()
                .HasMany(e => e.TBLSERVICEs)
                .WithRequired(e => e.TYPE)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TBLPAGE>()
                .Property(e => e.AboutText)
                .IsUnicode(false);

            modelBuilder.Entity<TBLPAGE>()
                .Property(e => e.DiscountParagraph)
                .IsUnicode(false);

            modelBuilder.Entity<EmpService>()
                .Property(e => e.TimeSpent)
                .HasPrecision(0);

            modelBuilder.Entity<EmpService>()
                .Property(e => e.DiscountedPrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<PaketHizmet>()
                .Property(e => e.DiscountedPrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ReservationInfo>()
                .Property(e => e.TimeSpent)
                .HasPrecision(0);

            modelBuilder.Entity<ReservationInfo>()
                .Property(e => e.CompletionDateTime)
                .HasPrecision(0);

            modelBuilder.Entity<ServiceType>()
                .Property(e => e.ServicePrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ServiceType>()
                .Property(e => e.DiscountedPrice)
                .HasPrecision(8, 2);

            modelBuilder.Entity<ServiceType>()
                .Property(e => e.TimeSpent)
                .HasPrecision(0);

            modelBuilder.Entity<TekilHizmetler>()
                .Property(e => e.DiscountedPrice)
                .HasPrecision(8, 2);
        }
    }
}
