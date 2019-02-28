using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialPlanner.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string RecipientId { get; set; }
        public DateTime Created { get; set; }
        public string Body { get; set; }
        public bool Read { get; set; }

        public virtual ApplicationUser Recipient { get; set; }
    }
}