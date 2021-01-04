using System;
using System.Collections.Generic;
using System.Text;

namespace Zwitter
{
    internal class Menu
    {
        public bool ShowMenu()
        {
            string[] options = new string[] { "Create new post", "Show all posts", "Update a post", "Delete a post", "Quit" };
            int input = UserIO.Menu(options, "Zwitter");

            PostManager postManager = new PostManager();

            switch (input)
            {
                case 0:
                    // Create new post

                    postManager.CreateNewPost();
                    return true;

                case 1:
                    //show all posts

                    postManager.DisplayPosts();
                    Console.ReadLine();
                    return true;

                case 2:
                    //update a post
                    return true;

                case 3:
                    // delete a post
                    return true;

                case 4:
                    //quit
                    return false;

                default:
                    Console.WriteLine("Give a vallid input plz!");
                    Console.ReadLine();
                    return true;
            }
        }
    }
}