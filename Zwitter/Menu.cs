using System;

namespace Zwitter
{
    internal class Menu
    {
        private User activeUser;
        private UserManager userManager;
        private PostManager postManager;
        private UserIO userIO;

        public Menu()
        {
            activeUser = new User();
            userManager = new UserManager();
            postManager = new PostManager();
            userIO = new UserIO();
        }

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
            string[] options = new string[] { "Create new zweet", "Show all zweets", "Show my zweets", "Update a zweet", "Delete a zweet", "Like a zweet", "LogOut", "Quit" };

            int input = userIO.Menu(options, $"   Welcome {activeUser.FirstName} {activeUser.LastName}");

            switch (input)
            {
                case 0:
                    // Create new post
                    postManager.CreateNewPost(activeUser);
                    return true;

                case 1:
                    //show all posts
                    postManager.DisplayAllPosts();
                    Console.ReadLine();
                    return true;

                case 2:
                    //show all posts of user
                    postManager.DisplayUserPosts(activeUser);
                    Console.ReadLine();
                    return true;

                case 3:
                    //update a post
                    postManager.UpdatePost(activeUser);
                    return true;

                case 4:
                    // delete a post
                    postManager.DeletePost(activeUser);
                    return true;

                case 5:
                    // like a post
                    postManager.LikePost(activeUser);
                    return true;

                case 6:
                    // LogOut
                    userManager.Logout(activeUser);
                    return true;

                case 7:
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

            string[] options = new string[] { "Login", "Register", "Check some zweets", "Quit" };
            int input = userIO.Menu(options, "");

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
                    // Display all posts
                    postManager.DisplayAllPosts();
                    Console.ReadLine();
                    return true;

                case 3:
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