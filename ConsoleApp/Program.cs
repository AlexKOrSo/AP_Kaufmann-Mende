using System;
using Tools;
namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string OriginPath = PathFinder.FindOrigin(); // sucht nach .Index-Datei, speichert deren Pfad
            Console.WriteLine("Willkommen in der Konsolen-App zur Bildklassifizierung auf Grundlage von Machine Learning");

            bool IsValidKey = false;
            char PressedKey=' '; 
            while (!IsValidKey) 
            {
                Console.WriteLine("Möchten Sie (1) Bilder kategorisieren oder (2) das Modell neu trainieren?");
                IsValidKey = ConsoleTools.IsValidKey(ref PressedKey, 1);
            }

            if (PressedKey == '1') { } //Überleitung zur Bildklassifizierung
            else if (PressedKey == '2') { } //Überleitung zum Training

        }
    }
}
