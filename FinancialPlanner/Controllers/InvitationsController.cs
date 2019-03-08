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
using FinancialPlanner.Helpers;
using FinancialPlanner.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPlanner.Controllers
{
    [Authorize]
    public class InvitationsController : Controller
    {
        private UserRoleHelper userRoleHelper = new UserRoleHelper();
        private ApplicationDbContext db = new ApplicationDbContext();
        private HouseholdHelper householdHelper = new HouseholdHelper();


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
        public async Task<ActionResult> Create([Bind(Include = "Id,HouseholdId,Email")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                invitation.Created = DateTime.Now;
                invitation.Expires = DateTime.Now.AddDays(1);
                invitation.Key = Guid.NewGuid();

                db.Invitations.Add(invitation);
                db.SaveChanges();


                var callbackUrl = Url.Action("Accept", "Invitations", new { Id = invitation.Key}, protocol: Request.Url.Scheme);
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

        public ActionResult Accept(string id)
        {

            Invitation invitation = db.Invitations.FirstOrDefault(i => i.Key.ToString() == id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            var user = db.Users.FirstOrDefault(x => x.Email == invitation.Email);
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("AddToHouse", "Invitations", new { key = invitation.Key });
            }
            else if (user != null)
            {
                TempData["invited"] = invitation.Key;
                return RedirectToAction("Login", "Account");
            }
            else
            {
                TempData["invited"] = invitation.Key;
                return RedirectToAction("Register", "Account");
            }

        }


        public async Task<ActionResult> AddToHouse(Guid key)
        {
            var invitation = db.Invitations.FirstOrDefault(i => i.Key == key);
            if (invitation.Expires > DateTime.Now || invitation.Expired)
            {
                invitation.Expired = true;
                db.Entry(invitation).Property(x => x.Expired).IsModified = true;
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                user.HouseholdId = invitation.HouseholdId;
                db.SaveChanges();
                var role = userRoleHelper.ListUserRoles(userId).FirstOrDefault();
                if(role != null)
                {
                    userRoleHelper.RemoveUserFromRole(userId, role);

                }
                userRoleHelper.AddUsertoRole(userId, "Member");
                await householdHelper.ReauthorizeUserAsync(userId);

                TempData["New"] = "new";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
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
