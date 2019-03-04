using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancialPlanner.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int? HouseholdId { get; set; }
        [Display(Name = "First Name")]
        [Required,MaxLength(40),MinLength(1)]
        public string FirstName { get; set; }
        [Required,MaxLength(40),MinLength(1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required,MaxLength(40),MinLength(1)]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }

        public virtual Household Household { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }

        public ApplicationUser()
        {
            Accounts = new HashSet<Account>();
            Notifications = new HashSet<Notification>();

        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<FinancialPlanner.Models.Account> Accounts { get; set; }

        public System.Data.Entity.DbSet<FinancialPlanner.Models.Household> Households { get; set; }

        public System.Data.Entity.DbSet<FinancialPlanner.Models.Budget> Budgets { get; set; }

        public System.Data.Entity.DbSet<FinancialPlanner.Models.BudgetItem> BudgetItems { get; set; }

        public System.Data.Entity.DbSet<FinancialPlanner.Models.Invitation> Invitations { get; set; }

        public System.Data.Entity.DbSet<FinancialPlanner.Models.Notification> Notifications { get; set; }

        public System.Data.Entity.DbSet<FinancialPlanner.Models.Transaction> Transactions { get; set; }

        public System.Data.Entity.DbSet<FinancialPlanner.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}