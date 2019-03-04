using FinancialPlanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}