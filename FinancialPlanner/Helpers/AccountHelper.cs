using FinancialPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPlanner.Helpers
{
    public class AccountHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdHelper householdHelper = new HouseholdHelper();

        public bool CanUserAccess(string userId, int accountId )
        {
            var account = db.Accounts.Find(accountId);
            if(account.UserId == userId)
            {
                return true;
            }
            if (account.HouseholdId != null)
            {
                try
                {
                    var users = householdHelper.GetHouseholdUsers((int)account.HouseholdId);
                    foreach (var user in users)
                    {
                        if(user.Id == userId)
                        {
                            return true;
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }
                return false;

        }
    }
}