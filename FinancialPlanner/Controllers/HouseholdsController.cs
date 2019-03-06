using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FinancialPlanner.Helpers;
using FinancialPlanner.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPlanner.Controllers
{
    public class HouseholdsController : Controller
    {
        private UserRoleHelper userRoleHelper = new UserRoleHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdHelper householdHelper = new HouseholdHelper();

        // GET: Households
        public ActionResult Index()
        {
            return View(db.Households.ToList());
        }

        public ActionResult Details()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            Household household = db.Households.Find(user.HouseholdId);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Greeting")] Household household)
        {
            if (ModelState.IsValid)
            {
                household.Key = Guid.NewGuid();
                db.Households.Add(household);
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                user.HouseholdId = household.Id;
     
                db.SaveChanges();

                userRoleHelper.AddUsertoRole(userId, "Head");
                await householdHelper.ReauthorizeUserAsync(userId);
                return RedirectToAction("Details");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Greeting")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> LeaveHousehold(int Id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if(user == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var houseHold = db.Households.Find(Id);
            if (houseHold == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            houseHold.Users.Remove(user);
            var role = userRoleHelper.ListUserRoles(userId).FirstOrDefault();
            userRoleHelper.RemoveUserFromRole(userId, role);

            userRoleHelper.AddUsertoRole(userId, "none");
            db.SaveChanges();

            await userRoleHelper.ReauthorizeUserAsync(userId);
            return RedirectToAction("Index", "Home");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
