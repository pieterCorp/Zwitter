using System;
using System.Collections.Generic;

namespace Zwitter
{
    internal class User
    {
        private int userId { get; set; }
        private string userName { get; set; }
        private DateTime dateRegistered { get; set; }
        private List<Post> allPosts = new List<Post>();
    }
}