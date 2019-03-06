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
        public int? getUserHousehold(int Id)
        {
            
            var user = db.Users.Find(Id);
            var houshold = user.HouseholdId; 
            return houshold;
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