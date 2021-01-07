using System;
using System.Linq;
using System.Threading;

namespace Zwitter
{
    internal class UserIO
    {
        internal string zwitterAscii = @"

::::::::: :::       ::: ::::::::::: ::::::::::: ::::::::::: :::::::::: :::::::::
     :+:  :+:       :+:     :+:         :+:         :+:     :+:        :+:    :+:
    +:+   +:+       +:+     +:+         +:+         +:+     +:+        +:+    +:+
   +#+    +#+  +:+  +#+     +#+         +#+         +#+     +#++:++#   +#++:++#:
  +#+     +#+ +#+#+ +#+     +#+         +#+         +#+     +#+        +#+    +#+
 #+#       #+#+# #+#+#      #+#         #+#         #+#     #+#        #+#    #+#
#########   ###   ###   ###########     ###         ###     ########## ###    ###

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
                        PadLeft("Input not valid, plz try again", 2, ConsoleColor.DarkRed);
                    }
                }
                catch (Exception)
                {
                    PadLeft("Input not valid, plz try again", 2, ConsoleColor.DarkRed);
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
                    PadLeft("Input not valid, plz try again", 2, ConsoleColor.DarkRed);
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
                PrintMenuOptions(options, subTitle, selectionIndex);
                ScreenSaver("Zwitter", 10, 100);
                PrintMenuOptions(options, subTitle, selectionIndex);

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

        private void PrintMenuOptions(string[] options, string subTitle, int selectionIndex)
        {
            Console.Clear();
            Console.CursorVisible = false;
            CenterText(zwitterAscii);
            PrintColor(ConsoleColor.DarkCyan, subTitle);

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
        }

        public void PrintColor(ConsoleColor color, string stringToPrint, bool writeLine = true)
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

        public void CenterText(string textToCenter)
        {
            Console.Clear();
            string[] lines;
            try
            {
                lines = textToCenter.Split(new[] { "\n" }, StringSplitOptions.None);
            }
            catch (Exception)
            {
                lines = textToCenter.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            }

            int longestLength = lines.Max(line => line.Length);
            string leadingSpaces = new string(' ', (Console.WindowWidth - longestLength) / 2);
            string centeredText = string.Join(Environment.NewLine,
                lines.Select(line => leadingSpaces + line));

            PrintColor(ConsoleColor.Cyan, centeredText);
        }

        public void PadLeft(string text, int indentation, ConsoleColor consoleColor = ConsoleColor.White)
        {
            Console.ForegroundColor = consoleColor;
            Console.CursorLeft = indentation;
            Console.WriteLine(text);
            Console.CursorLeft = indentation;
            Console.ResetColor();
        }

        private void ScreenSaver(string text, int setTimeInSec = 10, int speed = 50)
        {
            Console.BufferWidth = Console.WindowWidth;
            Console.BufferHeight = Console.WindowHeight;
            int Width = Console.BufferWidth - text.Length;
            int Height = Console.BufferHeight - 2;

            ConsoleColor[] color = { ConsoleColor.Red, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Green, ConsoleColor.DarkMagenta, ConsoleColor.Yellow, ConsoleColor.White };

            int counter = 0;
            int x = 0;
            int y = 0;

            bool xLeft = false;
            bool yUp = false;

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(10 * setTimeInSec);
                if (Console.KeyAvailable)
                {
                    return;
                }
            }

            while (!Console.KeyAvailable) //get out of loop when the any key is found and pressed
            {
                Console.CursorVisible = false;
                if (counter == color.Length - 1) counter = 0;

                if (x == Width)
                {
                    xLeft = true;
                    counter++;
                }
                else if (x == 1)
                {
                    xLeft = false;
                    counter++;
                }
                if (y == Height)
                {
                    yUp = true;
                    counter++;
                }
                else if (y == 1)
                {
                    yUp = false;
                    counter++;
                }

                _ = xLeft == true ? x-- : x++;
                _ = yUp == true ? y-- : y++;

                Console.Clear();
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = color[counter];
                Console.WriteLine(text);

                Thread.Sleep(speed);
            }
            Console.ReadKey();
        }
    }
}