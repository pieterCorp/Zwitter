using System;
using System.Linq;

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
                        PadLeft("Input not valid, plz try again", 2, ConsoleColor.Red);
                    }
                }
                catch (Exception)
                {
                    PadLeft("Input not valid, plz try again", 2, ConsoleColor.Red);
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
                    PadLeft("Input not valid, plz try again", 2, ConsoleColor.Red);
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

            var longestLength = lines.Max(line => line.Length);
            string leadingSpaces = new string(' ', (Console.WindowWidth - longestLength) / 2);
            var centeredText = string.Join(Environment.NewLine,
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
    }
}