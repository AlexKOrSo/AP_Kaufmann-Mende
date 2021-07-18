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
                //Abfangen des Keys, der angeschlagen wurden und Ausgabe auf Console.
                key = Console.ReadKey(false); 
                //Leerzeile
                Console.WriteLine();
            } while (key.Key!= ConsoleKey.Y && key.Key!=ConsoleKey.N); //So oft nachgefragt, bis y oder n (bzw. Y oder N)

            return (key.Key == ConsoleKey.Y); //Rückgabe Ergebnis
        }
    }
}
