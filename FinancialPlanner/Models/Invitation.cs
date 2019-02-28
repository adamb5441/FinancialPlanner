using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPlanner.Models
{
    public class Invitation
    {
        public int Id { get; set; }
        public int HouseholdId { get; set; }
        [Required]
        public Guid Key { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [MaxLength(200),MinLength(1)]
        string Body { get; set; }

        public DateTime Created { get; set; }
        public DateTime Expires { get; set; }

        public bool Expired { get; set; }
        public virtual Household Household { get; set; }
    }
}