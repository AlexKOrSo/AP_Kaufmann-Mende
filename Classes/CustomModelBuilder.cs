using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using MLData;
using Tools;
using System.IO; 


namespace Classes
{
    public class CustomBuilder
    {
        public static List<Dataset> Labels { get; private set; }
        static CustomBuilder()
        {
            Labels = new List<Dataset>();
        }
        public static void Initialization(string path)
        {
            {

                try
                {
                    DataCollection Data = new DataCollection(path, 500);
                    bool run = true;
                    
                    while (run)
                    {
                        //List<Dataset> labels;
                        Console.WriteLine("Bitte Text eingeben, der in der Kategoriebezeichnung enthalten sein soll: ");
                        Labels = Data.FindLables(Console.ReadLine());
                        foreach (Dataset item in Labels)
                        {
                            Console.WriteLine("{0}: {1}: {2}", Labels.IndexOf(item), item.Key, item.Label);
                        }
                        int[] index = ConsoleTools.VarInput("Bitte Kategorienummer eingeben  oder -1, um Eingabe neuzustarten, bei mehreren mit Leerzeichen getrennt");
                        Console.WriteLine(index.Length);


                        foreach (var item in index)
                        {
                            if (item == -1)
                            {
                                break;
                            }
                            else if (!Data.Labels.Contains(new Dataset(Labels[item].Key, Labels[item].Label)))
                            {
                                Data.Labels.Add(Labels[item]);
                            }

                            //labels.TryGetValue(item, out Dataset temp);
                            //Data.Labels.Add(temp);
                        }

                        run = ConsoleTools.YesNoInput("Nach neuer Kategorie suchen");

                    }
                    if (Data.Labels.Count < 2)
                    {
                        throw new Exception("Zu wenig Kategorien ausgewählt");
                    }
                    Data.DownloadAllDatasets(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Alles auf Anfang");
                }
            }
            //Löschen der Temporäeren Dateien fehlt noch, implementiere ich erst, wenn wir ganz sicher sind, dass auch der richtige dateipfad bei tools rauskommt ;)
            //Kommt raus :D
        }

        public static IEnumerable<Image> ImageCollector()
        {
            foreach (var Label in Labels)
            {
                string LabelFolder = Path.Combine(PathFinder.ImageDir, Label.Label);
                var TrainingImages = Directory.GetFiles(LabelFolder, "*.jpg");

                foreach (string SingleImage in TrainingImages)
                {
                    string FileName = Path.GetFullPath(SingleImage);
                    yield return new Image(FileName, Label.Label);
                }
            }
        }
    }
}
