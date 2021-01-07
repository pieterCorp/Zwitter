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

            int input = userIO.Menu(options, $"   Welcome {activeUser.UserName}");

            switch (input)
            {
                case 0:
                    postManager.CreateNewPost(activeUser);
                    return true;

                case 1:
                    postManager.DisplayAllPosts(activeUser);
                    Console.ReadLine();
                    return true;

                case 2:
                    postManager.DisplayUserPosts(activeUser);
                    Console.ReadLine();
                    return true;

                case 3:
                    postManager.UpdatePost(activeUser);
                    return true;

                case 4:
                    postManager.DeletePost(activeUser);
                    return true;

                case 5:
                    postManager.LikePost(activeUser);
                    return true;

                case 6:
                    userManager.Logout(activeUser);
                    return true;

                case 7:
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
                    activeUser = userManager.Login();
                    return true;

                case 1:
                    userManager.Register();
                    return true;

                case 2:
                    postManager.DisplayAllPosts(activeUser);
                    Console.ReadLine();
                    return true;

                case 3:
                    return false;

                default:
                    Console.WriteLine("Give a vallid input plz!");
                    Console.ReadLine();
                    return true;
            }
        }
    }
}