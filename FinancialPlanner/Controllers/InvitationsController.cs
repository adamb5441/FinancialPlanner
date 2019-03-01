using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FinancialPlanner.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPlanner.Controllers
{
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public ActionResult Index()
        {
            var invitations = db.Invitations.Include(i => i.Household);
            return View(invitations.ToList());
        }

        // GET: Invitations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // GET: Invitations/Create
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,HouseholdId,Key,Email,Created,Expires,Expired")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                invitation.Created = DateTime.Now;
                invitation.Expires = DateTime.Now.AddDays(1);

                db.Invitations.Add(invitation);
                db.SaveChanges();


                var callbackUrl = Url.Action("Accept", "Invitation", new { Id = invitation .Id}, protocol: Request.Url.Scheme);
                var from = "FiinancialPlanner<Fin@Plan.com>";
                var emailto = invitation.Email;
                var email = new MailMessage(from, emailto)
                {
                    Subject = "You have an invitation to join a household",
                    Body = "Join the household by clicking <a href=\"" + callbackUrl + "\">here</a>",
                    IsBodyHtml = true
                };

                var svc = new PersonalEmail();
                await svc.SendAsync(email);

                return RedirectToAction("Index","Home");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            return View(invitation);
        }

        // GET: Invitations/Edit/5
        public ActionResult Accept(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", invitation.HouseholdId);
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Accept(int Id)
        {
            
                var invitation = db.Invitations.Find(Id);
                invitation.Expired = true;
                db.Entry(invitation).Property(x => x.Expired).IsModified = true;
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                user.HouseholdId = invitation.HouseholdId;
                db.SaveChanges();

                return RedirectToAction("Index");
            
        }

        // GET: Invitations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
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
