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
using FinancialPlanner.Enumeration;
using FinancialPlanner.Helpers;

namespace FinancialPlanner.Controllers
{
    [Authorize]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private AccountHelper accountHelper = new AccountHelper();
        private HouseholdHelper householdHelper = new HouseholdHelper();

        // GET: Transactions
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var transactions = db.Transactions.Where(t => t.Account.HouseholdId == user.HouseholdId).Include(t => t.Account).Include(t => t.BudgetItem).Include(t => t.enteredBy);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            var householdId = householdHelper.getUserHousehold(User.Identity.GetUserId());
            var accounts = db.Accounts.Where(a => a.HouseholdId == householdId);
            ViewBag.Accountnum = accounts.Count();
            ViewBag.AccountId = new SelectList(accounts, "Id", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,Amount")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.enteredById = User.Identity.GetUserId();
                transaction.date = DateTime.Now;
                transaction.Type = TransactionTypes.Deposit;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                accountHelper.updateCurrentBalance(transaction.AccountId);
                return RedirectToAction("Index", "Home");
            }

            var householdId = householdHelper.getUserHousehold(User.Identity.GetUserId());
            ViewBag.AccountId = new SelectList(db.Accounts.Where(a => a.HouseholdId == householdId), "Id", "Name", transaction.AccountId);
            ViewBag.BudgetItemId = new SelectList(db.BudgetItems, "Id", "Name", transaction.BudgetItemId);
            ViewBag.enteredById = new SelectList(db.Users, "Id", "FirstName", transaction.enteredById);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Amount,Type,Reaconciled,ReconciledAmout")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).Property(c => c.Amount).IsModified = true;
                db.Entry(transaction).Property(c => c.Type).IsModified = true;
                db.Entry(transaction).Property(c => c.Reaconciled).IsModified = true;
                db.Entry(transaction).Property(c => c.ReconciledAmout).IsModified = true;
                db.SaveChanges();
                accountHelper.updateCurrentBalance(transaction.AccountId);
                return RedirectToAction("Index", "Home");
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            accountHelper.updateCurrentBalance(transaction.AccountId);
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Index");
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
