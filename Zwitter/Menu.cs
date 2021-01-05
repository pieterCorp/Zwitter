using System;

namespace Zwitter
{
    internal class Menu
    {
        private User activeUser = new User();

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
            UserManager userManager = new UserManager();
            PostManager postManager = new PostManager();

            string[] options = new string[] { "Posts", "LogOut", "Quit" };
            int input = UserIO.Menu(options, $"Zwitter logged in as {activeUser.FirstName} {activeUser.LastName}");

            switch (input)
            {
                case 0:
                    //Posts
                    bool inPostMenu = true;

                    while (inPostMenu)
                    {
                        inPostMenu = postManager.Menu();
                    }
                    return true;

                case 1:
                    // LogOut
                    userManager.Logout(activeUser);
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

        private bool MenuLoggedOut()
        {
            UserManager userManager = new UserManager();

            string[] options = new string[] { "Login", "Register", "Quit" };
            int input = UserIO.Menu(options, "Zwitter");

            switch (input)
            {
                case 0:
                    //login
                    activeUser = userManager.Login();
                    if (activeUser.LoggedIn)
                    {
                        Console.WriteLine($"Welcome {activeUser.FirstName} {activeUser.LastName}");
                        Console.ReadLine();
                    }
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

        public bool ShowMenu()
        {
            string[] options = new string[] { "posts", "Login", "Logout", "Register", "Quit" };
            int input = UserIO.Menu(options, "Zwitter");

            UserManager userManager = new UserManager();
            User activeUser = new User();

            switch (input)
            {
                case 0:
                    // posts
                    PostManager postManager = new PostManager();
                    bool inPostMenu = true;

                    while (inPostMenu)
                    {
                        inPostMenu = postManager.Menu();
                    }
                    return true;

                case 1:
                    //login
                    activeUser = userManager.Login();
                    Console.WriteLine($"Welcome {activeUser.FirstName} {activeUser.LastName}");
                    Console.ReadLine();
                    return true;

                case 2:
                    //logout
                    userManager.Logout(activeUser);
                    return true;

                case 3:
                    // register
                    userManager.Register();
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