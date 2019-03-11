using FinancialPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FinancialPlanner.Enumeration;

namespace FinancialPlanner.Helpers
{
    public class AccountHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdHelper householdHelper = new HouseholdHelper();

        public void getCurrentBalance(int Id)
        {
            var account = db.Accounts.Find(Id);
            var balance = 0.0m;
            foreach(Transaction transaction in account.Transactions)
            {
                if (transaction.Type == TransactionTypes.Withdraw && transaction.Reaconciled)
                {
                    balance -= transaction.ReconciledAmout;
                }
                if (transaction.Type == TransactionTypes.Withdraw)
                {
                    balance -= transaction.ReconciledAmout;
                }
                if (transaction.Type == TransactionTypes.Deposit && transaction.Reaconciled)
                {
                    balance += transaction.ReconciledAmout;
                }
                if (transaction.Type == TransactionTypes.Deposit)
                {
                    balance += transaction.ReconciledAmout;
                }
            }
            account.CurrentBalance = balance;
            db.SaveChanges();
            
        }

    }
}