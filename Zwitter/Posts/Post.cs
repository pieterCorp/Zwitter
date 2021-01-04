using System;
using System.Collections.Generic;
using System.Text;

namespace Zwitter
{
    internal class Post
    {
        public int postId { get; set; }
        private int postFromUserId { get; set; }
        public string postContent { get; set; }
        private DateTime postedAt { get; set; }
    }
}