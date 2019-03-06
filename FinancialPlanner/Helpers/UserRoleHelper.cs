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
    public class UserRoleHelper
    {
        private UserManager<ApplicationUser> userManager = new
        UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();

        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }
        public ICollection<string> ListUserRoles(string userId)
        {
            return userManager.GetRoles(userId);
        }
        public bool AddUsertoRole(string userId, string roleName)
        {
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }
        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }
        public ICollection<ApplicationUser> UsersInRole(string roleName)
        {
            var resultlist = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (IsUserInRole(user.Id, roleName))
                {
                    resultlist.Add(user);
                }
            }
            return resultlist;
        }
        public ICollection<ApplicationUser> UsersNotInRole(string roleName)
        {
            var resultlist = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach (var user in List)
            {
                if (!IsUserInRole(user.Id, roleName))
                {
                    resultlist.Add(user);
                }
            }
            return resultlist;
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