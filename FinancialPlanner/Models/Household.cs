using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPlanner.Models
{
    public class Household
    {
        public int Id { get; set;  }
        [Required, MaxLength(40), MinLength(1)]
        public string Name { get; set; }
        [Required, MaxLength(40), MinLength(0)]
        public string Greeting { get; set; }
        public Guid Key { get; set; }

        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Invitation> Invitations { get; set; }

        public Household()
        {
            Budgets = new HashSet<Budget>();
            Users = new HashSet<ApplicationUser>();
            Accounts = new HashSet<Account>();
            Invitations = new HashSet<Invitation>();
        }

    }
}