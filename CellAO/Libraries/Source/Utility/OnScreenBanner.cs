using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
    public static class OnScreenBanner
    {
        private static void CenteredString(string text, string boundary, ConsoleColor c = ConsoleColor.Black)
        {
            int consoleWidth = Console.WindowWidth;
            // Mono "fix"
            if (consoleWidth == 0)
            {
                // Since Console.WindowWidth doesnt work on mono, lets assume 80 chars
                consoleWidth = 80;
            }

            ConsoleColor oldColor = Console.ForegroundColor;
            int centered = (consoleWidth - text.Length) / 2;
            Console.Write(boundary.PadRight(centered, ' '));

            if (c != ConsoleColor.Black)
            {
                Console.ForegroundColor = c;
            }

            Console.Write(text);
            Console.ForegroundColor = oldColor;
            Console.Write(boundary.PadLeft(consoleWidth - (text.Length + centered), ' '));
        }

        public static void PrintCellAOBanner()
        {
            int consoleWidth = Console.WindowWidth;

            Console.Clear();

            Console.Write("**".PadRight(consoleWidth, '*'));
            CenteredString("", "**");
            CenteredString(AssemblyInfoclass.Title, "**", ConsoleColor.White);
            CenteredString(AssemblyInfoclass.AssemblyVersion, "**", ConsoleColor.DarkGreen);
            CenteredString(AssemblyInfoclass.RevisionName, "**", ConsoleColor.DarkGray);
            CenteredString("", "**");
            Console.Write("**".PadRight(consoleWidth, '*'));
        }

    }
}
