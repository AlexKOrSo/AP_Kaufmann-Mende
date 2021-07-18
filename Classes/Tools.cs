using System;

namespace Tools
{
    public static class ConsoleTools
    {
        public static bool YesNoInput(string question)
        {
            ConsoleKeyInfo pressedKey;
            do
            {
                Console.Write(question + @" [y/n]");
                //Abfangen des Keys, der angeschlagen wurden und Ausgabe auf Console.
                pressedKey = Console.ReadKey(false); 
                //Leerzeile
                Console.WriteLine();
            } while (pressedKey.Key!= ConsoleKey.Y && key.Key!=ConsoleKey.N); //So oft nachgefragt, bis y oder n (bzw. Y oder N)

            return (pressedKey.Key == ConsoleKey.Y); //Rückgabe Ergebnis
        }
    }
}
