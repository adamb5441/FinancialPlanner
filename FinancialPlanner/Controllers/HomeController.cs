using FinancialPlanner.Helpers;
using FinancialPlanner.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinancialPlanner.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private UserRoleHelper userRoleHelper = new UserRoleHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdHelper householdHelper = new HouseholdHelper();
        private BudgetHelper budgetHelper = new BudgetHelper();
        public ActionResult Index()
        {
            var householdId = householdHelper.getUserHousehold(User.Identity.GetUserId());
            if(householdId != null)
            {
                var model = db.Households.Find(householdId);
                foreach(var budget in model.Budgets)
                {
                    budgetHelper.updateBudget(budget.Id);
                }
                return View(model);
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}