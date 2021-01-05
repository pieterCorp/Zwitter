using System;
using static System.Console;

namespace Zwitter
{
    internal class UserIO
    {
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
                        PrintColor(ConsoleColor.Red, "Input not valid, plz try again");
                    }
                }
                catch (Exception)
                {
                    PrintColor(ConsoleColor.Red, "Input not valid, plz try again");
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
                    PrintColor(ConsoleColor.Red, "Input not valid, plz try again");
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
                        PrintColor(ConsoleColor.Red, "Input not valid, plz try again");
                    }
                }
                catch (Exception)
                {
                    PrintColor(ConsoleColor.Red, "Input not valid, plz try again");
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
                        PrintColor(ConsoleColor.Red, "Input not valid, plz try again");
                    }
                }
                catch (Exception)
                {
                    PrintColor(ConsoleColor.Red, "Input not valid, plz try again");
                }
            }
            return validatedInput;
        }

        static public int Menu(string[] options, string title)
        {
            ConsoleKey keyPressed;
            int selectionIndex = 0;

            do
            {
                Clear();
                Console.CursorVisible = false;
                PrintColor(ConsoleColor.Red, title);
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
                        ForegroundColor = ConsoleColor.Blue;
                        if (i == options.Length - 1) ForegroundColor = ConsoleColor.DarkRed;
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

        static public void PrintColor(ConsoleColor color, string stringToPrint)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(stringToPrint);
            Console.ResetColor();
        }
    }
}