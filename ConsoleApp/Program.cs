using System;
using Tools;
using MLData;
using System.Collections.Generic;
namespace ConsoleApp
{
    class Program
    {
        public static void Training(string path)
        {
            DataCollection Data = new DataCollection(path);
            bool run = true;
            
            while (run)
            {
                List<Dataset> labels;
                Console.WriteLine("Bitte Text eingeben, der in der Kategoriebezeichnung enthalten sein soll: ");
                labels = Data.FindLables(Console.ReadLine());
                foreach (Dataset item in labels)
                {
                    Console.WriteLine("{0}: {1}: {2}",labels.IndexOf(item), item.Key,item.Label);
                }
                int[] index = ConsoleTools.VarInput("Bitte Kategorienummer eingeben  oder -1, um Eingabe neuzustarten, bei mehreren mit Leerzeichen getrennt");

                
                foreach (var item in index)
                {
                    if (item == -1)
                    {
                        break;
                    }
                    else if(!Data.Labels.Contains(new Dataset(labels[item].Key,labels[item].Label)))
                    {
                        Data.Labels.Add(labels[item]);
                    }
                    
                    //labels.TryGetValue(item, out Dataset temp);
                    //Data.Labels.Add(temp);
                }

                run = ConsoleTools.YesNoInput("Nach neuer Kategorie suchen");
            }
                Data.downloadAllDatasets();
            //Löschen der Temporäeren Dateien fehlt noch, implementiere ich erst, wenn wir ganz sicher sind, dass auch der richtige dateipfad bei tools rauskommt ;)
        }

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

            if (PressedKey == '1') { 
                
            } //Überleitung zur Bildklassifizierung
            else if (PressedKey == '2') {
                Training(OriginPath);
            } //Überleitung zum Training

        }
    }
}
