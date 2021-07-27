using System;
using System.Collections.Generic;
using System.IO;

namespace MLData
{
    ///<include file='ClassesDoc/DataCollection.xml' path='DataCollection/Member[@name="DataCollection"]/*'/>
    public class DataCollection
    {
        ///<include file='ClassesDoc/DataCollection.xml' path='DataCollection/Member[@name="PathIDs"]/*'/>
        string PathIDs { get; set; } //Dateipfad zu CSV mit Bilder-IDs
        ///<include file='ClassesDoc/DataCollection.xml' path='DataCollection/Member[@name="PathLabels"]/*'/>
        string PathLabels { get; set; } //Dateipfad zu CSV mit Schlüsseln für entsprechende Labels
        ///<include file='ClassesDoc/DataCollection.xml' path='DataCollection/Member[@name="MaxItems"]/*'/>
        int MaxItems { get; set; }
        ///<include file='ClassesDoc/DataCollection.xml' path='DataCollection/Member[@name="Labels"]/*'/>
        public List<Dataset> Labels { get; set; }

        ///<include file='ClassesDoc/DataCollection.xml' path='DataCollection/Member[@name="FindLables"]/*'/>
        public List<Dataset> FindLables(string searchstring)
        {

            List<Dataset> results = new List<Dataset>();

            try
            {
                StreamReader sr = new StreamReader(PathLabels);
                string line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
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
                throw new Exception($"Fehler in {nameof(FindLables)}: " + e.Message);
            }
            return results;
        }
        ///<include file='ClassesDoc/DataCollection.xml' path='DataCollection/Member[@name="findImageIds"]/*'/>
        public void findImageIds()
        {
            try
            {
                StreamReader sr = new StreamReader(this.PathIDs);
                string line = sr.ReadLine();
                while ((line = sr.ReadLine()) != null)
                {
                    foreach (Dataset item in Labels)
                    {
                        if (line.Split(',')[2].Contains(item.Key) && line.Split(',')[3] == "1")
                        {
                            item.ids.Enqueue(line.Split(',')[0]);
                        }
                    }
                }
                sr.Dispose();
            }
            catch (Exception e)
            {
                Console.WriteLine();
                throw new Exception($"Fehler in {nameof(findImageIds)}: " + e.Message);
            }
        }
        ///<include file='ClassesDoc/DataCollection.xml' path='DataCollection/Member[@name="DownloadAllDatasets"]/*'/>
        public void DownloadAllDatasets(string path)
        {
            Console.WriteLine("Durchsuchen der ImageIDs......");
            findImageIds();
            foreach (Dataset item in Labels)
            {
                try
                {
                    string downloadpath = Path.Combine(path, "tmp", item.Label);
                    Console.WriteLine($"Dataset {item.Label} wird heruntergeladen nach: " + downloadpath);

                    if (!Directory.Exists(downloadpath))
                    {
                        Directory.CreateDirectory(downloadpath);
                    }

                    item.downloadAll(downloadpath, MaxItems);

                    Console.WriteLine($"Download abgeschlossen für Dataset {item.Label}");
                }
                catch (Exception e)
                {

                    throw new Exception($"Fehler in { nameof(DownloadAllDatasets)}: " + e.Message);
                }
            }


        }
        public DataCollection(string path, int maxItems)
        {
            this.PathIDs = Path.Combine(path, @"imageIDs.csv");
            this.PathLabels = Path.Combine(path, @"labels.csv");
            MaxItems = maxItems;
            Labels = new List<Dataset>();
            CheckFiles();
        }
        ///<include file='ClassesDoc/DataCollection.xml' path='DataCollection/Member[@name="CheckFiles"]/*'/>
        private void CheckFiles()
        {
            try
            {
                StreamReader labels = new StreamReader(this.PathLabels);
                StreamReader ids = new StreamReader(this.PathIDs);

                if (labels.ReadLine() != "LabelName,DisplayName" || ids.ReadLine() != "ImageID,Source,LabelName,Confidence")
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
