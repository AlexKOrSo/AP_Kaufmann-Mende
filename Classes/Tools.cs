using System;

namespace Tools
{
    public static class ConsoleTools
    {
        public static bool YesNoInput(string question)
        {
            ConsoleKeyInfo key;
            do
            {
                Console.Write(question + @" [y/n]");
                key = Console.ReadKey(false);
                Console.WriteLine();
            } while (key.Key!= ConsoleKey.Y && key.Key!=ConsoleKey.N);

            return (key.Key == ConsoleKey.Y);
        }
    }
}
