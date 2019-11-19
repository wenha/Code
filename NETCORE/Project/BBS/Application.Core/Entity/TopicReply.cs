using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core.Entity
{
    public class TopicReply : BaseEntity
    {
        public int TopicId { get; set; }
        public string ReplyUserId { get; set; }
        public User ReplyUser { get; set; }
        public string ReplyEmail { get; set; }
        public string ReplyContent { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
