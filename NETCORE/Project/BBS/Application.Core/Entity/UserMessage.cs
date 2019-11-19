using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Core.Entity
{
    public class UserMessage : BaseEntity
    {
        public string SendUserId { get; set; }
        public User SendUser { get; set; }
        public string ReceiveUserId { get; set; }
        public User ReceiveUser { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public int State { get; set; }
        public DateTime CreateOn { get; set; }
    }
}
