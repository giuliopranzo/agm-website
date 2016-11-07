using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class MessageReceiverMap : EntityTypeConfiguration<MessageReceiver>
    {
        public MessageReceiverMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.ToTable("Message_receivers");

            HasRequired(t => t.Message).WithMany().HasForeignKey(e => e.MessageId);
        }
    }
}