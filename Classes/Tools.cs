using System;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation;
using Classes.Properties;
using MLData; 
using System.Text;
using System.Net;
using System.Collections.Generic; 


namespace Tools
{
    public static class TSVMaker
    {
        public static readonly string LabelsData = Path.Combine(PathFinder.ImageDir, "Labels.tsv"); 
        static string AllData = Path.Combine(PathFinder.ImageDir, "AllData.tsv");
        public static readonly string TestData = Path.Combine(PathFinder.ImageDir, "TestData.tsv");
        public static readonly string TrainData = Path.Combine(PathFinder.ImageDir, "TrainData.tsv");
        public static string[] LabelNames; 
        public static int TestDataNumber;
        public static int TrainDataNumber; 

     
        public static void LogAllData(string LogPath, List<Dataset> Labels)
        {

            LabelNames = new string[Labels.Count];
            int Counter = 0; 
            foreach(var Element in Labels)
            {
                LabelNames[Counter] = Element.Label;
                Counter++; 
            }
            //Jetzt befinden sich alle Label-Namen als String-Array in "LabelNames"
            if(File.Exists(AllData)) File.Delete(AllData);
            if (File.Exists(TestData)) File.Delete(TestData);
            if (File.Exists(TrainData)) File.Delete(TrainData);


            int FileCounter = 0;
            try
            {
                using (StreamWriter AllWriter = new StreamWriter(AllData))
                using (StreamWriter TestWriter = new StreamWriter(TestData))
                using (StreamWriter TrainWriter = new StreamWriter(TrainData))


                    foreach (string Name in LabelNames)
                    {
                        string[] FilesInDir = Directory.GetFiles(Path.Combine(PathFinder.ImageDir, Name));
                        FileCounter = 0;
                        foreach (string File in FilesInDir)
                        {
                            string Output = File + ';' + Name;
                            if ((double)FileCounter < (double)0.8 * FilesInDir.Length) TrainWriter.WriteLine(Output);
                            else TestWriter.WriteLine(Output);
                            FileCounter++;
                            AllWriter.WriteLine(Output);
                        }
                    }
            }
            catch (Exception)
            {
                Console.WriteLine($"Es konnte mindestens eins der Files \n{AllData}\n{TestData}\n{TrainData}\n nicht geschrieben werden. Programmabbruch!");
                throw; 
            }
            

        }
        
    }


    public static class ConsoleTools
    {
        public static string NonEmptyInput()
        {//Fängt leere Eingaben (d.h. die nur aus Enter bestehen) ab
            string Input = null;
            while ((string.IsNullOrEmpty(Input)) ? true : false)
            {
                Input = Console.ReadLine();
                if(string.IsNullOrEmpty(Input)) Console.WriteLine("Enter ist keine gültige Eingabe!");
            }
            return Input;
        }
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
            string temp = ConsoleTools.NonEmptyInput(); 
            

            
                string[] line = temp.Split(' ');
                int[] input = new int[line.Length];
                bool ValidInput = true; 
                for (int i = 0; i < line.Length; i++)
                {
                    string item = line[i];
                    ValidInput=int.TryParse(item, out input[i]);
                if (!ValidInput) return null; 
                }
            

            return input;
        }
        public static bool FileNameInput(string FileName)
        {
            //Prüft, ob der FileName verwendet werden darf. 
            
            if(FileName.Contains(' ') || FileName.Contains('/') || FileName.Contains('\\'))
            {
                Console.WriteLine("Bitte Name ohne Leerzeichen oder (Back-)Slash eingeben");
                return false; 
            }
            return true; 
        }
    }

    public static class PathFinder
    {

        public readonly static string ModelDir=Path.Combine(PathFinder.FindOrigin(), "TensorFlow");
        //public static string ModelDir = Path.Combine(FindOrigin(), "Classes", "Model", "NewModel.pb"); //Platzhalter, soll durch User-Eingabe spezifiziert werden

        public readonly static  string ImageDir = Path.Combine(FindOrigin(), "tmp"); //Ordner, in dem die Bilder gespeichert werden

        public readonly static string OwnImagesDir = Path.Combine(FindOrigin(), "OwnImages");
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
