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
        private AccountHelper accountHelper = new AccountHelper();
        public ActionResult Index()
        {
            var householdId = householdHelper.getUserHousehold(User.Identity.GetUserId());
            if(householdId != null)
            {
                var model = db.Households.Find(householdId);
                foreach(var budget in model.Budgets)
                {
                    budgetHelper.updateBudget(budget.Id);
                    if (budgetHelper.isOverBudget(budget.Id))
                    {
                        @TempData["warning"] = $"{budget.Name} is over budget!";
                    }
                }
                foreach(var account in model.Accounts)
                {
                    if (accountHelper.isUnderLimit(account.Id))
                    {
                        @TempData["warning"] = $"{account.Name} is under your set level!";

                    }
                    if (accountHelper.isOverDraft(account.Id))
                    {
                        @TempData["warning"] = $"{account.Name} is over drafted!";

                    }
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