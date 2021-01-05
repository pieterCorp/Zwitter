using System;
using static System.Console;

namespace Zwitter
{
    internal class UserIO
    {
        internal static string zwitterAscii = @"

 ________  ___       __   ___  _________  _________  _______   ________
|\_____  \|\  \     |\  \|\  \|\___   ___\\___   ___\\  ___ \ |\   __  \
 \|___/  /\ \  \    \ \  \ \  \|___ \  \_\|___ \  \_\ \   __/|\ \  \|\  \
     /  / /\ \  \  __\ \  \ \  \   \ \  \     \ \  \ \ \  \_|/_\ \   _  _\
    /  /_/__\ \  \|\__\_\  \ \  \   \ \  \     \ \  \ \ \  \_|\ \ \  \\  \|
   |\________\ \____________\ \__\   \ \__\     \ \__\ \ \_______\ \__\\ _\
    \|_______|\|____________|\|__|    \|__|      \|__|  \|_______|\|__|\|__|

";

        static public bool AskYesNoQ()
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

        static public string GetUserString()
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

        static public int GetUserInt(int minValue, int maxValue)
        {
            bool validInput = false;
            int validatedInput = 0;
            while (!validInput)
            {
                try
                {
                    validatedInput = Convert.ToInt32(Console.ReadLine());
                    if (validatedInput >= minValue && validatedInput <= maxValue)
                    {
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
            return validatedInput;
        }

        static public double GetUserDouble(int minValue, int maxValue)
        {
            bool validInput = false;
            double validatedInput = 0;
            while (!validInput)
            {
                try
                {
                    validatedInput = Convert.ToDouble(Console.ReadLine());
                    if (validatedInput >= minValue && validatedInput <= maxValue)
                    {
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
            return validatedInput;
        }

        static public int Menu(string[] options)
        {
            ConsoleKey keyPressed;
            int selectionIndex = 0;

            do
            {
                Clear();
                Console.CursorVisible = false;
                PrintColor(ConsoleColor.Cyan, zwitterAscii, true);
                Console.WriteLine();

                for (int i = 0; i < options.Length; i++)
                {
                    if (selectionIndex == i)
                    {
                        ForegroundColor = ConsoleColor.White;
                        Write("-> ");
                    }
                    else
                    {
                        ForegroundColor = ConsoleColor.DarkCyan;
                        if (i == options.Length - 1) ForegroundColor = ConsoleColor.DarkGray;
                        Write("   ");
                    }

                    Console.WriteLine(options[i]);
                }
                ResetColor();

                ConsoleKeyInfo keyInfo = ReadKey(true);
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

        static public void PrintColor(ConsoleColor color, string stringToPrint, bool writeLine)
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
    }
}