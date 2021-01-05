using System;
using System.Collections.Generic;
using System.Text;

namespace Zwitter
{
    internal class Post
    {
        public int postId { get; set; }
        public int postFromUserId { get; set; }
        public string postContent { get; set; }
        public DateTime postedAt { get; set; }
    }
}