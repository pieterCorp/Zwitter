using System;

namespace Zwitter
{
    internal class Menu
    {
        private User activeUser = new User();
        private UserManager userManager = new UserManager();
        private PostManager postManager = new PostManager();

        public void RunApp()
        {
            bool runApp = true;

            while (runApp)
            {
                while (activeUser.LoggedIn && runApp)
                {
                    runApp = MenuLoggedIn();
                }
                while (!activeUser.LoggedIn && runApp)
                {
                    runApp = MenuLoggedOut();
                }
            }
        }

        private bool MenuLoggedIn()
        {
            string[] options = new string[] { "Create new post", "Show all posts", "Update a post", "Delete a post", "LogOut", "Quit" };
            int input = UserIO.Menu(options, $"Welcome {activeUser.FirstName} {activeUser.LastName}");

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
                    postManager.UpdatePost();
                    return true;

                case 3:
                    // delete a post
                    postManager.DeletePost();
                    return true;

                case 4:
                    // LogOut
                    userManager.Logout(activeUser);
                    return true;

                case 5:
                    //quit
                    return false;

                default:
                    Console.WriteLine("Give a vallid input plz!");
                    Console.ReadLine();
                    return true;
            }
        }

        private bool MenuLoggedOut()
        {
            UserManager userManager = new UserManager();

            string[] options = new string[] { "Login", "Register", "Quit" };
            int input = UserIO.Menu(options, "");

            switch (input)
            {
                case 0:
                    //login
                    activeUser = userManager.Login();
                    return true;

                case 1:
                    // register
                    userManager.Register();
                    return true;

                case 2:
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