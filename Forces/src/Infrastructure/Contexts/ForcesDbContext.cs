using Forces.Application.Enums;
using Forces.Application.Interfaces.Services;
using Forces.Application.Models;
using Forces.Application.Models.Chat;
using Forces.Domain.Contracts;
using Forces.Domain.Entities.Catalog;
using Forces.Domain.Entities.ExtendedAttributes;
using Forces.Domain.Entities.Misc;
using Forces.Infrastructure.Models;
using Forces.Infrastructure.Models.Identity;
using Forces.Infrastructure.Models.Identity.UserTypes;
using Forces.Infrastructure.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Data;
namespace Forces.Infrastructure.Contexts
{
    public class ForcesDbContext : AuditableContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;
        private Appuser currentUser;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ForcesDbContext(DbContextOptions<ForcesDbContext> options, ICurrentUserService currentUserService, IDateTimeService dateTimeService, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            // this.Database.EnsureCreated();
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
           currentUser = base.Users.FirstOrDefault(x => x.Id == _currentUserService.UserId);
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<ChatHistory<Appuser>> ChatHistories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Application.Models.Forces> Forces { get; set; }
        public DbSet<Bases> Bases { get; set; }
        public DbSet<BasesSections> BasesSections { get; set; }
        public DbSet<DepoDepartment> DepoDepartment { get; set; }
        public DbSet<HQDepartment> HQDepartment { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<MprRequest> MprRequests { get; set; }
        public DbSet<MprRequestAction> MprRequestAction { get; set; }
        public DbSet<MprRequestAttachments> MprRequestAttachment { get; set; }

        public DbSet<VoteCodes> VoteCodes { get; set; }
        // public DbSet<VoteCodeHolders> VoteCodeHolders { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Requests<VoteCodes, Appuser, RequestActions>> Requests { get; set; }
        public DbSet<RequestAttachments<VoteCodes, Appuser, RequestActions>> RequestAttachments { get; set; }
        public DbSet<Models.RequestActions> RequestActions { get; set; }
        public DbSet<MeasureUnits> MeasureUnits { get; set; }
        public DbSet<NPRReguest> NPRReguests { get; set; }
        public DbSet<BODReguest> BODReguests { get; set; }
        public DbSet<CashReguest> CashReguests { get; set; }
        public DbSet<VoteCodeLog> VoteCodeLog { get; set; }
        public DbSet<NotificationsUsers> NotificationsUsers { get; set; }
        public DbSet<DocumentExtendedAttribute> DocumentExtendedAttributes { get; set; }
        public DbSet<PersonalItems> PersonalItems { get; set; }
        public DbSet<PersonalItemsOperation_Hdr> PersonalItemsOperation_Hdr { get; set; }
        public DbSet<AirCraft> AirCraft { get; set; }
        public DbSet<PersonalItemsOperation_Details> PersonalItemsOperation_Details { get; set; }
        public DbSet<AirKind> AirKind { get; set; }
        public DbSet<AirType> AirType { get; set; }
        public DbSet<VehicleRequest> VehicleRequest { get; set; }
        public DbSet<Building> Building { get; set; }
        public DbSet<House> House { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<InventoryItem> InventoryItem { get; set; }
        public DbSet<InventoryItemBridge> InventoryItemBridge { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Room> Room { get; set; }



        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTimeService.NowUtc;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTimeService.NowUtc;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MprRequestAttachments>(entity => entity.ToTable("MprRequestAttachments"));

            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);

            builder.Entity<PersonalItems>().HasQueryFilter(x => currentUser.BaseID.HasValue ? x.BaseId == currentUser.BaseID : true);
            builder.Entity<Tailers>().HasQueryFilter(x => currentUser.BaseID.HasValue ? x.BaseId == currentUser.BaseID : true);

            builder.Entity<ChatHistory<Appuser>>(entity =>
            {
                entity.ToTable("ChatHistory");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatHistoryFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatHistoryToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            //builder.Entity<VoteCodeHolders>(entity =>
            //{
            //    // entity.HasKey(x => x.Id);
            //    entity.HasNoKey();
            //    entity.HasOne(v => v.User)
            //    .WithMany(p => p.HoldingVoteCodes)
            //    .HasForeignKey(x => x.UserId)
            //    .OnDelete(DeleteBehavior.Cascade);


            //    entity.HasOne(v => v.VoteCode)
            //    .WithMany(p => p.VoteCodeHolders)
            //    .HasForeignKey(x => x.VoteCodeId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //}

            //);
            builder.Entity<VoteCodes>(entity =>
            {
                entity.HasOne(x => x.DefaultHolder);

            });
            builder.Entity<Appuser>(entity =>
            {
                entity.ToTable(name: "Users", "Identity");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                // entity.HasDiscriminator<string>("UserType");
                entity.HasMany(x => x.HoldingVoteCodes).WithMany(x => x.VoteCodeHolders);


            });
            builder.Entity<Application.Models.Requests<VoteCodes, Appuser, RequestActions>>(entity =>
            {
                entity.HasDiscriminator<string>("RequestType");
            });
            builder.Entity<MeasureUnits>(entity =>
            {
                entity.HasMany(x => x.Items)
                .WithOne(x => x.MeasureUnit)
                .HasForeignKey(x => x.MeasureUnitId);
            });
            builder.Entity<AppRole>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity");

            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", "Identity");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");
            });

            builder.Entity<AppRoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
            });
        }
    }

}