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
        int MaxItems { get; set; } //Maximale Anzahl an Items, die heruntergeladen werden soll
        public List<Dataset> Labels { get; set; } //Liste der DataSet (pro Label ein Dataset)
        

        public List<Dataset> FindLables(string searchstring)
        {
            //neue Liste, die Suchergebnisse als jeweils eine neue Dataset-instanz enthält
            List<Dataset> results = new List<Dataset>(); 

            try
            {
                StreamReader sr = new StreamReader(PathLabels);
                string line = sr.ReadLine(); //Erste Zeile enthält Kopf, ist deshalb irrelevant
                Console.WriteLine("Test");
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Split(',')[1].ToUpper().Contains(searchstring.ToUpper()))
                    {
                        results.Add(new Dataset(line.Split(',')[0], line.Split(',')[1]));
                    }
                }
                sr.Dispose();
            }
            catch (Exception)
            {
                Console.WriteLine("Dateifehler");
                throw;
            }
            return results;
        }

        public void findImageIds()
        {
            try
            {
                StreamReader sr = new StreamReader(this.PathIDs);
                string line = sr.ReadLine(); //Erste Zeile enthält Kopf, ist deshalb irrelevant
                while ((line = sr.ReadLine()) != null)
                {
                    foreach (Dataset item in Labels)
                    {
                        if (line.Split(',')[2].Contains(item.Key)&&line.Split(',')[3]=="1")
                        {
                            item.ids.Enqueue(line.Split(',')[0]);
                        } 
                    }
                }
                sr.Dispose();
            }
            catch (Exception)
            {
                Console.WriteLine("Dateifehler");
                throw;
            }
        }
        public void DownloadAllDatasets(string path)
        {
            Console.WriteLine("IM here");
            findImageIds();
            foreach (Dataset item in Labels)
            {
                try
                {
                    string downloadpath = Path.Combine(path, "tmp", item.Label);
                    Console.WriteLine(downloadpath);

                    if (!Directory.Exists(downloadpath))
                    {
                        Directory.CreateDirectory(downloadpath);
                    }
                    
                    item.downloadAll(downloadpath);

                    Console.WriteLine("finished download");
                }
                catch (Exception)
                {
                    Console.WriteLine("Fehler");
                    throw;
                }
            }

           
        }
        public DataCollection(string path, int maxItems)
        {
            //Pfade sind nur vorübergehend festgeschrieben, hier müssen noch methoden zur überprüfung hin, ob datei existiert...
            //this.PathIDs = @"R:\Transfer\Softwareentwicklung_Github\python\oidv6-train-annotations-human-imagelabels.csv";
            //this.PathLabels = @"R:\Transfer\Softwareentwicklung_Github\python\oidv6-class-descriptions.csv";
            this.PathIDs = Path.Combine(path, @"oidv6-train-annotations-human-imagelabels.csv");
            this.PathLabels = Path.Combine(path, @"oidv6-class-descriptions.csv");
            MaxItems = maxItems;
            Labels = new List<Dataset>();
            CheckFiles();
        }

        private void CheckFiles()
        {
            try
            {
                StreamReader labels = new StreamReader(this.PathLabels);
                StreamReader ids = new StreamReader(this.PathIDs);
                
                if (labels.ReadLine() != "LabelName,DisplayName"||ids.ReadLine()!= "ImageID,Source,LabelName,Confidence")
                {
                    Console.WriteLine("CSV-Dateien nicht gültig!");
                    throw new Exception("Dateilfehler");
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            

        }

    }
}
