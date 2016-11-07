using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class MessageReceiver
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int ToUserId { get; set; }
        public bool IsArchived { get; set; }
        public bool IsDeleted { get; set; }
        public Message Message { get; set; }
    }
}