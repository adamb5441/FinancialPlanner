﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialPlanner.Models
{
    public class Account
    {
        public int Id { get; set; }
        public int? HouseholdId { get; set; }

        [Required, MaxLength(40), MinLength(1)]
        public string Name { get; set; }
        [Range(0, 100000000)]
        public Decimal InitialBalance { get; set; }
        public Decimal CurrentBalance { get; set; }

        public Decimal? LowBalanceLevel { get; set; }

        public virtual Household Household{get;set; }


        public ICollection<Transaction> Transactions { get; set; }

        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }
    }
}