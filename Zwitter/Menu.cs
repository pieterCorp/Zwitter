using System;
using System.Collections.Generic;
using System.Text;

namespace Zwitter
{
    internal class Menu
    {
        public bool ShowMenu()
        {
            string[] options = new string[] { "posts", "Login", "Logout", "Regeister", "Quit" };
            int input = UserIO.Menu(options, "Zwitter");

            switch (input)
            {
                case 0:
                    // posts
                    PostManager postManager = new PostManager();

                    while (postManager.Menu())
                    {
                        postManager.Menu();
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