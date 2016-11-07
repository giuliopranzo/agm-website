using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.ToTable("Messages");

            this.Ignore(t => t.Sender);
            this.Ignore(t => t.Receivers);

            //HasMany<MessageReceiver>(s => s.Receivers)
            //        .WithRequired(s => s.Message)
            //        .HasForeignKey(s => s.MessageId);
        }
    }
}