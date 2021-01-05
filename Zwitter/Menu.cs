using System;
using System.Collections.Generic;
using System.Text;

namespace Zwitter
{
    internal class Menu
    {
        public bool ShowMenu()
        {
            string[] options = new string[] { "Posts", "Login", "Logout", "Register", "Quit" };
            int input = UserIO.Menu(options);

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
                    return true;

                case 2:
                    //logout
                    return true;

                case 3:
                    // register
                    UserManager userManager = new UserManager();
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