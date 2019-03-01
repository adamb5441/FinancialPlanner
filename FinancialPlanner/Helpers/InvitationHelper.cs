using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;

namespace FinancialPlanner.Helpers
{
    public class InvitationHelper
    {
        public async Task InviteEmail(string emailto)
        {
            var from = "FinnancialPlanner<Fp@Tracker.com>";

        var email = new MailMessage(from, emailto)
        {
            Subject = "Ticket changes",
            Body = "You hav a household invitation",
            IsBodyHtml = true
        };
        var svc = new PersonalEmail();
        await svc.SendAsync(email);
        }
    }
}