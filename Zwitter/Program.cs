using System;

namespace Zwitter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.SetWindowSize(100, 25);
            Console.BufferHeight = 25;
            Console.BufferWidth = 100;
            Menu menu = new Menu();
            menu.RunApp();
        }
    }
}