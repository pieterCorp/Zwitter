using System;

namespace Zwitter
{
    internal class UserIO
    {
        internal string zwitterAscii = @"

 ________  ___       __   ___  _________  _________  _______   ________
|\_____  \|\  \     |\  \|\  \|\___   ___\\___   ___\\  ___ \ |\   __  \
 \|___/  /\ \  \    \ \  \ \  \|___ \  \_\|___ \  \_\ \   __/|\ \  \|\  \
     /  / /\ \  \  __\ \  \ \  \   \ \  \     \ \  \ \ \  \_|/_\ \   _  _\
    /  /_/__\ \  \|\__\_\  \ \  \   \ \  \     \ \  \ \ \  \_|\ \ \  \\  \|
   |\________\ \____________\ \__\   \ \__\     \ \__\ \ \_______\ \__\\ _\
    \|_______|\|____________|\|__|    \|__|      \|__|  \|_______|\|__|\|__|

";

        public bool AskYesNoQ()
        {
            bool validInput = false;
            bool answer = false;
            char input;
            while (!validInput)
            {
                try
                {
                    input = Convert.ToChar(Console.ReadLine());
                    if (input == 'y')
                    {
                        answer = true;
                        validInput = true;
                    }
                    else if (input == 'n')
                    {
                        answer = false;
                        validInput = true;
                    }
                    else
                    {
                        PrintColor(ConsoleColor.Red, "Input not valid, plz try again", true);
                    }
                }
                catch (Exception)
                {
                    PrintColor(ConsoleColor.Red, "Input not valid, plz try again", true);
                }
            }
            return answer;
        }

        public string GetUserString()
        {
            bool validInput = false;
            string input = "";

            while (!validInput)
            {
                try
                {
                    input = Console.ReadLine();
                    validInput = true;
                }
                catch (Exception)
                {
                    PrintColor(ConsoleColor.Red, "Input not valid, plz try again", true);
                }
            }

            return input;
        }

        public int Menu(string[] options, string subTitle)
        {
            ConsoleKey keyPressed;
            int selectionIndex = 0;

            do
            {
                Console.Clear();
                Console.CursorVisible = false;
                PrintColor(ConsoleColor.Cyan, zwitterAscii, true);
                PrintColor(ConsoleColor.DarkCyan, subTitle, true);

                Console.WriteLine();

                for (int i = 0; i < options.Length; i++)
                {
                    if (selectionIndex == i)
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("-> ");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        if (i == options.Length - 1) Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("   ");
                    }

                    Console.WriteLine(options[i]);
                }
                Console.ResetColor();

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectionIndex--;
                    if (selectionIndex < 0)
                    {
                        selectionIndex = options.Length - 1;
                    }
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectionIndex++;
                    if (selectionIndex > options.Length - 1)
                    {
                        selectionIndex = 0;
                    }
                }
            } while (keyPressed != ConsoleKey.Enter);

            Console.CursorVisible = true;
            return selectionIndex;
        }

        public void PrintColor(ConsoleColor color, string stringToPrint, bool writeLine)
        {
            Console.ForegroundColor = color;
            if (writeLine)
            {
                Console.WriteLine(stringToPrint);
            }
            else
            {
                Console.Write(stringToPrint);
            }
            Console.ResetColor();
        }

        public void ShowTitle()
        {
            Console.Clear();
            PrintColor(ConsoleColor.Cyan, zwitterAscii, true);
        }
    }
}