using System;
using System.IO;
using System.Threading;
using Classes.Properties;

namespace Tools
{
    public static class ConsoleTools
    {
        public static bool YesNoInput(string question)
        {
            ConsoleKeyInfo pressedKey;
            do
            {
                Console.Write(question + @" [y/n]: ");
                //Abfangen des Keys, der angeschlagen wurden und Ausgabe auf Console.
                pressedKey = Console.ReadKey(false); 
                //Leerzeile
                Console.WriteLine();
            } while (pressedKey.Key!= ConsoleKey.Y && pressedKey.Key!=ConsoleKey.N); //So oft nachgefragt, bis y oder n (bzw. Y oder N)

            return (pressedKey.Key == ConsoleKey.Y); //Rückgabe Ergebnis
        }
        public static bool IsValidKey(ref char PressedKey, byte Option)
        {
            ConsoleKeyInfo Choice = Console.ReadKey(true);
            PressedKey = Choice.KeyChar;
            switch (Option)
            {
                case 1: if (PressedKey == '1' || PressedKey == '2') return true; break; 
                //weitere mögliche Cases
                

            }
            return false; //äquivalent zu entsprechender default-Verzweigung im Switch-Block
        }
    
        public static int[] VarInput(string question)
        {
            Console.WriteLine(question);
            string[] line = Console.ReadLine().Split(' ');
            int[] input = new int[line.Length];
            for (int i = 0; i < line.Length; i++)
            {
                string item = line[i];
                int.TryParse(item, out input[i]);
            }
            
            return input;
        }
    }

    public static class PathFinder
    {
        public static string FindOrigin()
        {
            //string FileName = ".Index"; //.Index File ist im hierarchisch höchsten Ordner des Projekts
            string FileName = Resources.IndexFileName; 
            string SearchPath = "";
            bool FileFound = false;
            string CurrentDir = AppDomain.CurrentDomain.BaseDirectory.ToString();

            while (!FileFound)
            {
                SearchPath = Path.Combine(CurrentDir, FileName);
                //Console.WriteLine($"Searching For: {SearchPath}");
                if (File.Exists(SearchPath))
                {
                    //Console.WriteLine($"Index-File gefunden in {SearchPath}");
                    FileFound = true; 
                }
                else
                {
                    //Thread.Sleep(1000);
                    CurrentDir = Directory.GetParent(CurrentDir).ToString();
                }
            }
            return Path.GetDirectoryName(SearchPath); 
        }

        public  static DirectoryInfo  MakeDirectory(string DirName) { 
            ///Erstellt Directory relativ zu Verzeichnis der .Index-Datei
            ///

            string FinalPath=Path.Combine(FindOrigin(), DirName);
            DirectoryInfo NewDir=Directory.CreateDirectory(FinalPath); 
            return NewDir; 
            }
    }
}
