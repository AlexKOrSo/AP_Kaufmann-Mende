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
        public static string LabelsData = Path.Combine(PathFinder.ImageDir, "Labels.tsv"); 
        static string AllData = Path.Combine(PathFinder.ImageDir, "AllData.tsv");
        public static string TestData = Path.Combine(PathFinder.ImageDir, "TestData.tsv");
        public static string TrainData = Path.Combine(PathFinder.ImageDir, "TrainData.tsv");
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
            using (StreamWriter AllWriter = new StreamWriter(AllData))
            using (StreamWriter TestWriter = new StreamWriter(TestData)) 
            using(StreamWriter TrainWriter=new StreamWriter(TrainData))
            
       
            foreach (string Name in LabelNames)
            {
                    string[] FilesInDir = Directory.GetFiles(Path.Combine(PathFinder.ImageDir, Name));
                    FileCounter = 0;
                    foreach(string File in FilesInDir)
                    {
                        string Output = File + ';' + Name;
                        if ((double)FileCounter < (double) 0.4 * FilesInDir.Length) TrainWriter.WriteLine(Output);
                        else TestWriter.WriteLine(Output);
                        FileCounter++; 
                        AllWriter.WriteLine(Output);
                    }
            }
            

        }
        public static void TransferAllData()
        {
            
        }
    }


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
        public static string ModelDir = Path.Combine(FindOrigin(), "Classes", "Model", "NewModel.pb"); //Platzhalter, soll durch User-Eingabe spezifiziert werden
        public static string ImageDir = Path.Combine(FindOrigin(), "tmp"); //Ordner, in dem die Bilder gespeichert werden
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

    public static class CheckNetwork
    {
        public static bool PingAWS()
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();
            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            try
            {
                PingReply reply = pingSender.Send("quicksight.us-east-1.amazonaws.com", timeout, buffer, options);
            }
            catch (Exception)
            {

                return false;
            }
            return true;

        }
    }
}
