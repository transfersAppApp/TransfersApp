using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Entities;

namespace TransfersApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Здесь добавьте настраиваемые утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public static ApplicationDbContext _context;
        public DbSet<Travel> Travels { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Confirmation> Confirmations { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Wish> Wishes { get; set; }
        public DbSet<RallyPoint> RallyPoints { get; set; }
        public DbSet<TransferRallyPoint> TransferRallyPoints { get; set; }
        public DbSet<ClientTransfer> ClientTransfers { get; set; }
        public DbSet<ClientPayment> ClientPayments { get; set; }
        public DbSet<TravelStatus> TravelStatuses { get; set; }
        public DbSet<TransferStatus> TransferStatuses { get; set; }
        public DbSet<TransportClass> TransportClasses { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Shuttle> Shuttles { get; set; }
        public DbSet<TimeSlot> TimeSlots { get; set; }


        [InjectionConstructor]
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

    }
}