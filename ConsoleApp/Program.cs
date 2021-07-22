using System;
using Tools;
using System.IO; 

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Before Exceptiom"); 
            try { 
            string OriginPath = PathFinder.FindOrigin(); // sucht nach .Index-Datei, speichert deren Pfad
                }
            catch(Exception) {Console.WriteLine(@"Couldn't find .Index-File"); }
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

            DirectoryInfo TrainingDir=PathFinder.MakeDirectory("TrainingModel");
            Console.WriteLine(TrainingDir.FullName); 
        }
    }
}
