using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace A65Insurance.Models
{
    public partial class A45InsuranceContext : DbContext
    {
        public A45InsuranceContext()
        {
        }

        public A45InsuranceContext(DbContextOptions<A45InsuranceContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Claim> Claim { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Plan> Plan { get; set; }
        public virtual DbSet<Service> Service { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Claim>(entity =>
            {
                entity.ToTable("Claim");

                entity.Property(e => e.AdjustedClaimId)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.AdjustedDate).HasColumnType("datetime");

                entity.Property(e => e.AdjustingClaimId)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.AppAdjusting)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.BalanceOwed).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.ClaimDescription)
                    .HasMaxLength(100)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.ClaimIdNumber)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength(true);

                entity.Property(e => e.ClaimStatus)
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.ClaimType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasDefaultValueSql("('u')")
                    .IsFixedLength(true);

                entity.Property(e => e.Clinic)
                    .HasMaxLength(35)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.CoveredAmount).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.CustomerId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.DateAdded).HasColumnType("datetime");

                entity.Property(e => e.DateConfine).HasColumnType("datetime");

                entity.Property(e => e.DateRelease).HasColumnType("datetime");

                entity.Property(e => e.DateService).HasColumnType("datetime");

                entity.Property(e => e.Diagnosis1)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.Diagnosis2)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.DrugName)
                    .HasMaxLength(35)
                    .IsFixedLength(true);

                entity.Property(e => e.Eyeware)
                    .HasMaxLength(35)
                    .IsFixedLength(true);

                entity.Property(e => e.Location)
                    .HasMaxLength(35)
                    .IsFixedLength(true);

                entity.Property(e => e.PatientFirst)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.PatientLast)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsFixedLength(true);

                entity.Property(e => e.PaymentAction)
                    .HasMaxLength(15)
                    .IsFixedLength(true);

                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.Physician)
                    .HasMaxLength(35)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.PlanId)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsFixedLength(true);

                entity.Property(e => e.Procedure1)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.Procedure2)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.Procedure3)
                    .HasMaxLength(5)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.Referral)
                    .HasMaxLength(25)
                    .IsFixedLength(true);

                entity.Property(e => e.Service)
                    .HasMaxLength(30)
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.TotalCharge).HasColumnType("decimal(12, 2)");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.AppId)
                    .HasMaxLength(5)
                    .HasColumnName("appId")
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.ClaimCount)
                    .HasMaxLength(25)
                    .HasColumnName("claimCount")
                    .IsFixedLength(true);

                entity.Property(e => e.CustAddr1)
                    .HasMaxLength(35)
                    .HasColumnName("custAddr1")
                    .IsFixedLength(true);

                entity.Property(e => e.CustAddr2)
                    .HasMaxLength(35)
                    .HasColumnName("custAddr2")
                    .IsFixedLength(true);

                entity.Property(e => e.CustBirthDate)
                    .HasColumnType("datetime")
                    .HasColumnName("custBirthDate");

                entity.Property(e => e.CustCity)
                    .HasMaxLength(35)
                    .HasColumnName("custCity")
                    .IsFixedLength(true);

                entity.Property(e => e.CustEmail)
                    .HasMaxLength(30)
                    .HasColumnName("custEmail")
                    .IsFixedLength(true);

                entity.Property(e => e.CustFirst)
                    .HasMaxLength(20)
                    .HasColumnName("custFirst")
                    .IsFixedLength(true);

                entity.Property(e => e.CustGender)
                    .HasMaxLength(1)
                    .HasColumnName("custGender")
                    .IsFixedLength(true);

                entity.Property(e => e.CustId)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("custId")
                    .IsFixedLength(true);

                entity.Property(e => e.CustLast)
                    .HasMaxLength(20)
                    .HasColumnName("custLast")
                    .IsFixedLength(true);

                entity.Property(e => e.CustMiddle)
                    .HasMaxLength(20)
                    .HasColumnName("custMiddle")
                    .IsFixedLength(true);

                entity.Property(e => e.CustPassword)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("custPassword")
                    .IsFixedLength(true);

                entity.Property(e => e.CustPhone)
                    .HasMaxLength(20)
                    .HasColumnName("custPhone")
                    .IsFixedLength(true);

                entity.Property(e => e.CustPlan)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("custPlan")
                    .IsFixedLength(true);

                entity.Property(e => e.CustState)
                    .HasMaxLength(2)
                    .HasColumnName("custState")
                    .IsFixedLength(true);

                entity.Property(e => e.CustZip)
                    .HasMaxLength(15)
                    .HasColumnName("custZip")
                    .IsFixedLength(true);

                entity.Property(e => e.Encrypted)
                    .HasMaxLength(25)
                    .IsFixedLength(true);

                entity.Property(e => e.ExtendColors)
                    .HasMaxLength(5)
                    .HasColumnName("extendColors")
                    .HasDefaultValueSql("(' ')")
                    .IsFixedLength(true);

                entity.Property(e => e.PromotionCode)
                    .HasMaxLength(10)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Plan>(entity =>
            {
                entity.ToTable("Plan");

                entity.Property(e => e.Percent)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsFixedLength(true);

                entity.Property(e => e.PlanLiteral)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsFixedLength(true);

                entity.Property(e => e.PlanName)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.Property(e => e.ClaimType)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsFixedLength(true);

                entity.Property(e => e.ClaimTypeLiteral)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsFixedLength(true);

                entity.Property(e => e.Cost).HasColumnType("decimal(12, 2)");

                entity.Property(e => e.ServiceName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
