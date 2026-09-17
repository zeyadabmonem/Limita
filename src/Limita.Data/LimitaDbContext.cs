using Limita.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Limita.Data;

public class LimitaDbContext : DbContext
{
    public LimitaDbContext(DbContextOptions<LimitaDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Card> Cards => Set<Card>();
    public DbSet<Beneficiary> Beneficiaries => Set<Beneficiary>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ---------- User ----------
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(u => u.Id);
            e.Property(u => u.FullName).HasMaxLength(150).IsRequired();
            e.Property(u => u.Email).HasMaxLength(256).IsRequired();
            e.Property(u => u.PhoneNumber).HasMaxLength(20).IsRequired();
            e.Property(u => u.PasswordHash).HasMaxLength(255).IsRequired();
            e.Property(u => u.ProfileImage).HasMaxLength(500);
            e.Property(u => u.Language).HasMaxLength(10).IsRequired();
            e.Property(u => u.Currency).HasMaxLength(10).IsRequired();
            e.HasIndex(u => u.Email).IsUnique();
            e.HasIndex(u => u.PhoneNumber).IsUnique();
        });

        // ---------- Account ----------
        modelBuilder.Entity<Account>(e =>
        {
            e.ToTable("Accounts");
            e.HasKey(a => a.Id);
            e.Property(a => a.Balance).HasColumnType("decimal(18,2)");
            e.Property(a => a.Currency).HasMaxLength(10).IsRequired();
            e.Property(a => a.Status).HasConversion<string>().HasMaxLength(20);
            e.HasOne(a => a.User)
             .WithMany(u => u.Accounts)
             .HasForeignKey(a => a.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(a => a.UserId);
        });

        // ---------- Card ----------
        modelBuilder.Entity<Card>(e =>
        {
            e.ToTable("Cards");
            e.HasKey(c => c.Id);
            e.Property(c => c.CardName).HasMaxLength(100).IsRequired();
            e.Property(c => c.LastFourDigits).HasMaxLength(4).IsFixedLength().IsRequired();
            e.Property(c => c.Brand).HasMaxLength(20).IsRequired();
            e.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
            e.HasOne(c => c.Account)
             .WithMany(a => a.Cards)
             .HasForeignKey(c => c.AccountId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(c => c.AccountId);
        });

        // ---------- Beneficiary ----------
        modelBuilder.Entity<Beneficiary>(e =>
        {
            e.ToTable("Beneficiaries");
            e.HasKey(b => b.Id);
            e.Property(b => b.Name).HasMaxLength(150).IsRequired();
            e.Property(b => b.AccountIdentifier).HasMaxLength(100).IsRequired();
            e.HasOne(b => b.User)
             .WithMany(u => u.Beneficiaries)
             .HasForeignKey(b => b.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            // A user shouldn't have the same beneficiary account identifier twice
            e.HasIndex(b => new { b.UserId, b.AccountIdentifier }).IsUnique();
        });

        // ---------- Transaction ----------
        modelBuilder.Entity<Transaction>(e =>
        {
            e.ToTable("Transactions");
            e.HasKey(t => t.Id);
            e.Property(t => t.Amount).HasColumnType("decimal(18,2)");
            e.Property(t => t.Currency).HasMaxLength(10).IsRequired();
            e.Property(t => t.Type).HasConversion<string>().HasMaxLength(20);
            e.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
            e.Property(t => t.Reference).HasMaxLength(50).IsRequired();
            e.Property(t => t.Note).HasMaxLength(500);
            e.HasIndex(t => t.Reference).IsUnique();

            e.HasOne(t => t.User)
             .WithMany(u => u.Transactions)
             .HasForeignKey(t => t.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(t => t.Account)
             .WithMany(a => a.Transactions)
             .HasForeignKey(t => t.AccountId)
             .OnDelete(DeleteBehavior.Restrict);

            // Nullable FK: beneficiary transfers only
            e.HasOne(t => t.Beneficiary)
             .WithMany(b => b.Transactions)
             .HasForeignKey(t => t.BeneficiaryId)
             .OnDelete(DeleteBehavior.Restrict);

            // Nullable FK: bill payments only
            e.HasOne(t => t.Bill)
             .WithMany(b => b.Transactions)
             .HasForeignKey(t => t.BillId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(t => new { t.UserId, t.CreatedAt });

            // Business rule: amount must be positive (enforced again in the service layer)
            e.ToTable(tb => tb.HasCheckConstraint("CK_Transactions_Amount_Positive", "[Amount] > 0"));
        });

        // ---------- Bill ----------
        modelBuilder.Entity<Bill>(e =>
        {
            e.ToTable("Bills");
            e.HasKey(b => b.Id);
            e.Property(b => b.ProviderName).HasMaxLength(150).IsRequired();
            e.Property(b => b.BillNumber).HasMaxLength(100).IsRequired();
            e.Property(b => b.Amount).HasColumnType("decimal(18,2)");
            e.Property(b => b.Status).HasConversion<string>().HasMaxLength(20);
            e.HasOne(b => b.User)
             .WithMany(u => u.Bills)
             .HasForeignKey(b => b.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(b => b.UserId);
            e.ToTable(tb => tb.HasCheckConstraint("CK_Bills_Amount_Positive", "[Amount] > 0"));
        });

        // ---------- Notification ----------
        modelBuilder.Entity<Notification>(e =>
        {
            e.ToTable("Notifications");
            e.HasKey(n => n.Id);
            e.Property(n => n.Title).HasMaxLength(150).IsRequired();
            e.Property(n => n.Message).HasMaxLength(500).IsRequired();
            e.HasOne(n => n.User)
             .WithMany(u => u.Notifications)
             .HasForeignKey(n => n.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(n => new { n.UserId, n.IsRead });
        });
    }
}
