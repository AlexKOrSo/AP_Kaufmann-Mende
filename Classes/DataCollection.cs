using System;
using System.Collections.Generic;
using System.IO;
using Tools; 

namespace MLData
{
    public class DataCollection
    {
        string PathIDs { get; set; } //Dateipfad zu CSV mit Bilder-IDs
        string PathLabels { get; set; } //Dateipfad zu CSV mit Schlüsseln für entsprechende Labels
        int MaxItems { get; set; } //Maximale Anzahl an Items, die je Dataset heruntergeladen werden soll
        public List<Dataset> Labels { get; set; } //Liste der DataSet (ein Dataset entspricht einer Kategorie)
        

        public List<Dataset> FindLables(string searchstring) //Durchsucht labels.csv nach übereinstimmungen mit searchstring
        {
            //neue Liste, die Suchergebnisse als jeweils eine neue Dataset-instanz enthält
            List<Dataset> results = new List<Dataset>(); 

            try
            {
                StreamReader sr = new StreamReader(PathLabels);
                string line = sr.ReadLine(); //Erste Zeile enthält Kopf, ist deshalb irrelevant
                Console.WriteLine("Test");
                while ((line = sr.ReadLine()) != null) //Abbruch wenn Dateiende
                {
                    if (line.Split(',')[1].ToUpper().Contains(searchstring.ToUpper()))
                    {
                        results.Add(new Dataset(line.Split(',')[0], line.Split(',')[1]));
                    }
                }
                sr.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception($"Fehler in {nameof(FindLables)}: "+e.Message);
            }
            return results;
        }

        public void findImageIds() //DurchsuchT imageIDs.csv nach übereinstimmungen mit dem Label-Key
        {
            try
            {
                StreamReader sr = new StreamReader(this.PathIDs);
                string line = sr.ReadLine(); //Erste Zeile enthält Kopf, ist deshalb irrelevant
                while ((line = sr.ReadLine()) != null)//Lesen einer Zeile, Abbruch wenn dateiende
                {
                    foreach (Dataset item in Labels)//gelesene Zeile wird für jedes Dataset auf übereinstimmung überprüft, und ob Confidence für das Label==1
                    {
                        if (line.Split(',')[2].Contains(item.Key)&&line.Split(',')[3]=="1")
                        {
                            item.ids.Enqueue(line.Split(',')[0]); //Enqueuen der ID in die Queue des zugehörigen Datasets
                        } 
                    }
                }
                sr.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine();
                throw new Exception($"Fehler in {nameof(findImageIds)}: "+ e.Message);
            }
        }
        public void DownloadAllDatasets(string path) //Übergeordnete Steuerungsmethode der Datasets, bekommt Programmpfad übergeben
        {
            Console.WriteLine("Durchsuchen der ImageIDs......");
            findImageIds();
            foreach (Dataset item in Labels) //Jedes Dataset wird einzeln der Download aufgerufen
            {
                try
                {
                    string downloadpath = Path.Combine(path, "tmp", item.Label); 
                    Console.WriteLine($"Dataset {item.Label} wird heruntergeladen nach: "+downloadpath);

                    if (!Directory.Exists(downloadpath))
                    {
                        Directory.CreateDirectory(downloadpath);
                    }

                    item.downloadAll(downloadpath,MaxItems); //Starten des Downloads eines Datasets

                    Console.WriteLine($"Download abgeschlossen für Dataset {item.Label}");
                }
                catch (Exception e)
                {
                    
                    throw new Exception($"Fehler in { nameof(DownloadAllDatasets)}: "+e.Message);
                }
            }

           
        }
        public DataCollection(string path, int maxItems) //Constructor
        {
            this.PathIDs = Path.Combine(path, @"imageIDs.csv");
            this.PathLabels = Path.Combine(path, @"labels.csv");
            MaxItems = maxItems;
            Labels = new List<Dataset>(); //Die Liste der Datasets, die heruntergeladen werden sollen
            CheckFiles();
        }

        private void CheckFiles()
        {       //Überprüfen der csv-Dateien:  
            try
            {
                StreamReader labels = new StreamReader(this.PathLabels); //a) wenn sie nicht vorhanden sind, wirft StreamReader eine Exception,
                StreamReader ids = new StreamReader(this.PathIDs);
                
                if (labels.ReadLine() != "LabelName,DisplayName"||ids.ReadLine()!= "ImageID,Source,LabelName,Confidence") //b) Wenn sie vorhanden sind, aber der Header falsch ist, wird eine exception geworfen
                {
                    Console.WriteLine("");
                    throw new Exception("CSV-Dateien nicht gültig!");
                }
            }
            catch (Exception e)
            {
                throw new Exception("CSV-Datei(en) nicht gefunden: " + e.Message);
            }
            
            

        }

    }
}
