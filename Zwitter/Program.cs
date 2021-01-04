using System;

namespace Zwitter
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu menu = new Menu();

            bool appRunning = true;

            while (appRunning)
            {
                appRunning = menu.ShowMenu();
            }
        }
    }
}
