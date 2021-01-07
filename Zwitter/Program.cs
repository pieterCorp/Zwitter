using System;

namespace Zwitter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.SetWindowSize(150, 25);
            Console.BufferHeight = 25;
            Console.BufferWidth = 150;
            Menu menu = new Menu();
            menu.RunApp();
        }
    }
}