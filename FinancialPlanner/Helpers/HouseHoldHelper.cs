using FinancialPlanner.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FinancialPlanner.Helpers
{
    public class HouseholdHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public int? getUserHousehold(string Id)
        {
            
            var user = db.Users.Find(Id);
            var houshold = user.HouseholdId; 
            return houshold;
        }
        public bool CanUserAccess(string userId, int accountId)
        {
            var account = db.Accounts.Find(accountId);
            if (account.UserId == userId)
            {
                return true;
            }
            if (account.HouseholdId != null)
            {
                try
                {
                    var users = GetHouseholdUsers((int)account.HouseholdId);
                    foreach (var user in users)
                    {
                        if (user.Id == userId)
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
        public List<ApplicationUser> GetHouseholdUsers(int Id)
        {
            var users = db.Users.Where(x => x.HouseholdId == Id);
            return users.ToList();
        }
        public async Task ReauthorizeUserAsync(string userId)
        {
            IAuthenticationManager authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            var user = userManager.FindById(userId);
            var identity = await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = true }, identity);
        }
    }
}