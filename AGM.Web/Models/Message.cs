using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public DateTime InsertDate { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }
        public string Sender { get; set; }
        public string Receivers { get; set; }
        public List<int> ReceiverIds { get; set; }
        //public virtual ICollection<MessageReceiver> Receivers { get; set; }
    }
}