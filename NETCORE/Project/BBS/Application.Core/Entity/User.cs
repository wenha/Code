﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Application.Core.Entity
{
    public class User : IdentityUser
    {
        public string Avatar { get; set; }
        public string Profile { get; set; }
        public string Url { get; set; }
        public string GitHub { get; set; }
        public int TopicCount { get; set; }
        public int TopicReplyCount { get; set; }
        public int Score { get; set; }
        public DateTime CreateOn { get; set; }
        public DateTime LastTime { get; set; }
    }
}
