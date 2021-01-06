using System;
using System.Collections.Generic;
using System.Text;

namespace Zwitter
{
    internal class Post
    {
        public int PostId { get; set; }
        public int PostFromUserId { get; set; }
        public int[] LikedBy { get; set; }
        public string PostContent { get; set; }
        public DateTime PostedAt { get; set; }
    }
}