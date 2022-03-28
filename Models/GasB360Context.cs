using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GasB360_server.Models
{
    public partial class GasB360Context : DbContext
    {
        public GasB360Context()
        {
        }

        public GasB360Context(DbContextOptions<GasB360Context> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAddress> TblAddresses { get; set; } = null!;
        public virtual DbSet<TblBranch> TblBranches { get; set; } = null!;
        public virtual DbSet<TblCustomer> TblCustomers { get; set; } = null!;
        public virtual DbSet<TblEmployee> TblEmployees { get; set; } = null!;
        public virtual DbSet<TblFilledProduct> TblFilledProducts { get; set; } = null!;
        public virtual DbSet<TblOrder> TblOrders { get; set; } = null!;
        public virtual DbSet<TblProductCategory> TblProductCategories { get; set; } = null!;
        public virtual DbSet<TblRole> TblRoles { get; set; } = null!;
        public virtual DbSet<TblType> TblTypes { get; set; } = null!;
        public virtual DbSet<TblUnfilledProduct> TblUnfilledProducts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:gasb360.database.windows.net,1433;Initial Catalog=GasB360;Persist Security Info=False;User ID=admin@gasb360@gasb360;Password=Kovai.co;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAddress>(entity =>
            {
                entity.HasKey(e => e.AddressId)
                    .HasName("PK__tbl_addr__CAA247C866B32566");

                entity.ToTable("tbl_address");

                entity.Property(e => e.AddressId)
                    .HasColumnName("address_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.AddressCity)
                    .IsUnicode(false)
                    .HasColumnName("address_city");

                entity.Property(e => e.AddressCountry)
                    .IsUnicode(false)
                    .HasColumnName("address_country");

                entity.Property(e => e.AddressPincode)
                    .IsUnicode(false)
                    .HasColumnName("address_pincode");

                entity.Property(e => e.AddressState)
                    .IsUnicode(false)
                    .HasColumnName("address_state");

                entity.Property(e => e.AddressStreetName)
                    .IsUnicode(false)
                    .HasColumnName("address_street_name");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblAddresses)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__tbl_addre__custo__1EA48E88");
            });

            modelBuilder.Entity<TblBranch>(entity =>
            {
                entity.HasKey(e => e.BranchId)
                    .HasName("PK__tbl_bran__E55E37DE67C95B19");

                entity.ToTable("tbl_branch");

                entity.Property(e => e.BranchId)
                    .HasColumnName("branch_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.AdminId).HasColumnName("admin_id");

                entity.Property(e => e.BranchLocation)
                    .IsUnicode(false)
                    .HasColumnName("branch_location");

                entity.Property(e => e.BranchName)
                    .IsUnicode(false)
                    .HasColumnName("branch_name");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.TblBranches)
                    .HasForeignKey(d => d.AdminId)
                    .HasConstraintName("FK__tbl_branc__admin__19DFD96B");
            });

            modelBuilder.Entity<TblCustomer>(entity =>
            {
                entity.HasKey(e => e.CustomerId)
                    .HasName("PK__tbl_cust__CD65CB85B668ACF1");

                entity.ToTable("tbl_customer");

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.AllowedLimit)
                    .HasColumnName("allowed_limit")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CustomerConnection)
                    .HasColumnName("customer_connection")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.CustomerEmail)
                    .IsUnicode(false)
                    .HasColumnName("customer_email");

                entity.Property(e => e.CustomerName)
                    .IsUnicode(false)
                    .HasColumnName("customer_name");

                entity.Property(e => e.CustomerPhone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("customer_phone");

                entity.Property(e => e.Password)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Requested)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("requested")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblCustomers)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__tbl_custo__role___0C85DE4D");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.TblCustomers)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__tbl_custo__type___0E6E26BF");
            });

            modelBuilder.Entity<TblEmployee>(entity =>
            {
                entity.HasKey(e => e.EmployeeId)
                    .HasName("PK__tbl_empl__C52E0BA831F62699");

                entity.ToTable("tbl_employee");

                entity.Property(e => e.EmployeeId)
                    .HasColumnName("employee_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.EmployeeEmail)
                    .IsUnicode(false)
                    .HasColumnName("employee_email");

                entity.Property(e => e.EmployeeName)
                    .IsUnicode(false)
                    .HasColumnName("employee_name");

                entity.Property(e => e.EmployeePhone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("employee_phone");

                entity.Property(e => e.Password)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblEmployees)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK__tbl_emplo__role___1332DBDC");
            });

            modelBuilder.Entity<TblFilledProduct>(entity =>
            {
                entity.HasKey(e => e.FilledProductId)
                    .HasName("PK__tbl_fill__EB5887737CA38D55");

                entity.ToTable("tbl_filled_product");

                entity.Property(e => e.FilledProductId)
                    .HasColumnName("filled_product_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.BranchId).HasColumnName("branch_id");

                entity.Property(e => e.FilledProductQuantity)
                    .HasColumnName("filled_product_quantity")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_id");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.TblFilledProducts)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK__tbl_fille__branc__2BFE89A6");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.TblFilledProducts)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK__tbl_fille__produ__29221CFB");
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.HasKey(e => e.OrderId)
                    .HasName("PK__tbl_orde__465962297F81316D");

                entity.ToTable("tbl_order");

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CustomerId).HasColumnName("customer_id");

                entity.Property(e => e.EmployeeId).HasColumnName("employee_id");

                entity.Property(e => e.OrderDate)
                    .HasColumnType("datetime")
                    .HasColumnName("order_date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.OrderOtp)
                    .HasColumnName("order_otp")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.OrderStatus)
                    .IsUnicode(false)
                    .HasColumnName("order_status");

                entity.Property(e => e.OrderTotalprice)
                    .HasColumnName("order_totalprice")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductId).HasColumnName("product_id");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__tbl_order__custo__37703C52");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__tbl_order__emplo__395884C4");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.TblOrders)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("FK__tbl_order__produ__3864608B");
            });

            modelBuilder.Entity<TblProductCategory>(entity =>
            {
                entity.HasKey(e => e.ProductId)
                    .HasName("PK__tbl_prod__47027DF51316B4F6");

                entity.ToTable("tbl_product_category");

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.ProductName)
                    .IsUnicode(false)
                    .HasColumnName("product_name");

                entity.Property(e => e.ProductPrice)
                    .HasColumnName("product_price")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ProductWeight)
                    .HasColumnName("product_weight")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TypeId).HasColumnName("type_id");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.TblProductCategories)
                    .HasForeignKey(d => d.TypeId)
                    .HasConstraintName("FK__tbl_produ__type___245D67DE");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__tbl_role__760965CCCA250A40");

                entity.ToTable("tbl_role");

                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.RoleType)
                    .IsUnicode(false)
                    .HasColumnName("role_type");
            });

            modelBuilder.Entity<TblType>(entity =>
            {
                entity.HasKey(e => e.TypeId)
                    .HasName("PK__tbl_type__2C000598940EC1AC");

                entity.ToTable("tbl_type");

                entity.Property(e => e.TypeId)
                    .HasColumnName("type_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.TypeName)
                    .IsUnicode(false)
                    .HasColumnName("type_name");
            });

            modelBuilder.Entity<TblUnfilledProduct>(entity =>
            {
                entity.HasKey(e => e.UnfilledProductId)
                    .HasName("PK__tbl_unfi__C72B47F182AAF618");

                entity.ToTable("tbl_unfilled_product");

                entity.Property(e => e.UnfilledProductId)
                    .HasColumnName("unfilled_product_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Active)
                    .HasMaxLength(5)
                    .IsUnicode(false)
                    .HasColumnName("active")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.BranchId).HasColumnName("branch_id");

                entity.Property(e => e.ProductCategoryId).HasColumnName("product_category_id");

                entity.Property(e => e.UnfilledProductQuantity)
                    .HasColumnName("unfilled_product_quantity")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.TblUnfilledProducts)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK__tbl_unfil__branc__32AB8735");

                entity.HasOne(d => d.ProductCategory)
                    .WithMany(p => p.TblUnfilledProducts)
                    .HasForeignKey(d => d.ProductCategoryId)
                    .HasConstraintName("FK__tbl_unfil__produ__2FCF1A8A");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
