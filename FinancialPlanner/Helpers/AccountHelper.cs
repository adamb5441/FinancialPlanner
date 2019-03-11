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

        public void updateCurrentBalance(int Id)
        {
            var account = db.Accounts.Find(Id);
            var transactions = db.Transactions.Where(t => t.AccountId == Id);
            var balance = account.InitialBalance;
            foreach(var transaction in transactions)
            {
                if (transaction.Type == TransactionTypes.Withdraw && transaction.Reaconciled)
                {
                    balance = balance - transaction.ReconciledAmout;
                }
                if (transaction.Type == TransactionTypes.Withdraw)
                {
                    balance = balance - transaction.Amount;
                }
                if (transaction.Type == TransactionTypes.Deposit && transaction.Reaconciled)
                {
                    balance = balance + transaction.ReconciledAmout;
                }
                if (transaction.Type == TransactionTypes.Deposit)
                {
                    balance = balance + transaction.Amount;
                }
            }
            account.CurrentBalance = balance;
            db.SaveChanges();
            
        }
        public bool isOverDraft(int accountId)
        {
            var account = db.Accounts.Find(accountId);
            if(account.CurrentBalance < 0)
            {
                return true;
            }
            return false;
        }
        public bool isUnderLimit(int accountId)
        {
            var account = db.Accounts.Find(accountId);
            if (account.CurrentBalance < account.LowBalanceLevel)
            {
                return true;
            }
            return false;
        }
    }
}