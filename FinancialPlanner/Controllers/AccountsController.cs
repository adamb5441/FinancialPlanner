using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPlanner.Models;
using Microsoft.AspNet.Identity;
using FinancialPlanner.Helpers;

namespace FinancialPlanner.Controllers
{
    [Authorize]
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private AccountHelper accountHelper = new AccountHelper();
        // GET: Accounts
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var accounts = db.Accounts.Include(a => a.Household);
                return View(accounts.ToList());
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                var accounts = db.Accounts.Where(x => user.HouseholdId == x.HouseholdId).Include(a => a.Household);
                return View(accounts.ToList());
            }
            
        }

        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Include(a=>a.Transactions).FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,InitialBalance,LowBalanceLevel")] Account account)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var HouseholdId = db.Users.Find(userId).HouseholdId;
                account.HouseholdId = HouseholdId;
                account.CurrentBalance = account.InitialBalance;
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            
            return View(account);
        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,LowBalanceLevel")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Attach(account);
                db.Entry(account).Property(x => x.Name).IsModified = true;
                db.Entry(account).Property(x => x.LowBalanceLevel).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(account);
        }

        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
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
