using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellAO.Relay.Common
{
    public static class ConsoleUtilities
    {
        public static void WriteError(string message, params string[] args)
        {
            UseTextColour(ConsoleColor.Red, () => Console.Error.WriteLine(message, args));
        }

        public static void UseTextColour(ConsoleColor colour, Action action)
        {
            var prevForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = colour;
            action();
            Console.ForegroundColor = prevForegroundColor;
        }
    }
}
