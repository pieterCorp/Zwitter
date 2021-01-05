using System;
using System.Collections.Generic;

namespace Zwitter
{
    internal class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool LoggedIn { get; set; }
        public DateTime dateRegistered { get; set; }
        public List<Post> allPosts = new List<Post>();
    }
}