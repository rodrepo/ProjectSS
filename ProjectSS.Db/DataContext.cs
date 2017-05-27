using Microsoft.AspNet.Identity.EntityFramework;
using ProjectSS.Db.Contracts;
using ProjectSS.Db.Entities;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectSS.Db
{
    public class DataContext : IdentityDbContext<User>
    {
        public string UserId { get; set; }
        public DataContext()
          : base("DefaultConnection")
        { }

        public DataContext(string userId)
          : base("DefaultConnection")
        {
            UserId = userId;
        }

        public static DataContext Create()
        {
            return new DataContext();
        }

        #region Override Methods
        public override Task<int> SaveChangesAsync()
        {
            AuditChanges();
            return base.SaveChangesAsync();
        }

        public override int SaveChanges()
        {
            AuditChanges();
            return base.SaveChanges();
        }

        private void AuditChanges()
        {
            var changeSet = ChangeTracker.Entries<IAuditable>();
            if (changeSet != null)
            {
                foreach (var entry in changeSet.Where(c => c.State != EntityState.Unchanged))
                {
                    if (entry.State == EntityState.Added)
                    {
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.CreatedBy = UserId;
                    }
                    entry.Entity.ModifiedDate = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = UserId;
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins");

            //modelBuilder.Entity<Practitioner>().HasMany(p => p.Facilities).WithMany(p => p.Practitioners).Map(m => { m.MapLeftKey("PractitionerId"); m.MapRightKey("FacilityId"); m.ToTable("PositionPermissions"); });
            //modelBuilder.Entity<User>().HasMany(u => u.Accounts).WithMany(p => p.Users).Map(m => { m.MapLeftKey("UserId"); m.MapRightKey("AccountId"); m.ToTable("UserAccounts"); });
            //modelBuilder.Entity<User>().HasMany(u => u.Positions).WithMany(p => p.Users).Map(m => { m.MapLeftKey("UserId"); m.MapRightKey("PositionId"); m.ToTable("UserPositions"); });
            //modelBuilder.Entity<Subscription>().HasMany(u => u.Features).WithMany(p => p.Subscriptions).Map(m => { m.MapLeftKey("SubscriptionId"); m.MapRightKey("FeatureId"); m.ToTable("SubscriptionFeatures"); });
        }
        #endregion

        #region DBSets / Tables      
        public DbSet<IdentityUserRole> UserRoles { get; set; }
        public DbSet<CRM> CRMs { get; set; }
        public DbSet<CRMEmailHistory> CRMEmailHistorys { get; set; }
        public DbSet<CRMCallHistory> CRMCallHistorys { get; set; }
        public DbSet<CRMRevisionHistory> CRMRevisionHistorys { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<ProposalStaff> ProposalStaffs { get; set; }
        public DbSet<ProposalContractor> ProposalContractors { get; set; }
        public DbSet<ProposalExpense> ProposalExpensess { get; set; }
        public DbSet<ProposalEquipment> ProposalEquipments { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<ProposalLaboratory> ProposalLaboratories { get; set; }
        public DbSet<ProposalCommission> ProposalCommissions { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<BudgetRequest> BudgetRequests { get; set; }
        public DbSet<BudgetRequestItem> BudgetRequestItems { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
        #endregion

    }
}


