using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Tools;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace MLData
{
    public class Dataset: IEquatable<Dataset>  //Für vergleichsoperationen wird IEquatable implementiert
    {
        public string Label { get; set; } //Bezeichnung der kategorie
        public string Key { get; set; } //Schlüssel, der Label zugeordnet ist
        public ConcurrentQueue<string> ids; //Asynchrone Queue

        public class counterHolder //Zähler heruntergeladener Bilder, extra-class, da async-Taks darauf zugreifen müssen und diese z. B. keine ref int counter als Parameter übernehmen
        {
            public int Value;
        }

        async Task DownloadFilesAsync(string path, counterHolder counter)
        {
            
            string bucketName = "open-images-dataset";      //Cloudspeicher(Bucket) von AWS, Zugriff mit Bucket-Namen
            string filename;

            RegionEndpoint reg = RegionEndpoint.USEast1; //Regionendpoint von AWS, Einstiegspunkt für AWS-Service
            AmazonS3Client s3Client = null;
            int timeout=30;
            try
            {
                s3Client = new AmazonS3Client(null, new AmazonS3Config() { RegionEndpoint = reg, Timeout = TimeSpan.FromSeconds(timeout) }); //anonymer AmazonAWSs-Client, der Informationen zum Zugriff aus S3-Dienste beinhaltet
                                                                                                                                             //im Pythonsript der Datenbank muss kein Regionendpoint angegeben werden...
            }
            catch (Exception e )
            {
                throw new Exception("AWS - Config - Fehler: "+ e.Message);
            }
            Console.WriteLine($"Download wird gestartet, Timeout beträgt {timeout} Sekunden");
            var fileTransferUtility = new TransferUtility(s3Client); //TransferUtility stellt API für UPload/Download zu S3-Diensten bereit

            string temp;
            while (ids.TryDequeue(out temp)) //Asynchrones dequeuen der ConcurrentQueue ids, gibt false zurück, wenn queue leer
            {

                filename = Path.Combine(path,(temp +".jpg")); //Alle Dateien sind jpgs
                Console.WriteLine($"Bilder in Queue: {ids.Count.ToString()}");

              
               try{

                   
                    await fileTransferUtility.DownloadAsync(filename, bucketName, "train/" + temp + ".jpg");
                    int currentCounter=Interlocked.Decrement(ref counter.Value); //Threadsicher decremenet des Counters
                    System.Console.WriteLine($"Zahl herunterzuladender Bilder: {currentCounter}");
                    if (currentCounter<=0)
                    {
                        break;//Beenden der Task, wenn counter 0/negativ ist
                    }

                }
              catch (Amazon.S3.AmazonS3Exception e) {
                    Console.WriteLine($"Bild {temp} not found: " + e.Message);
                }

              catch (Amazon.Runtime.AmazonServiceException e)
                {
                    Console.WriteLine(e.Message+" "+e.GetType());
                    throw new Exception($"Schwerwiegender Fehler in {nameof(DownloadFilesAsync)}: " + e.GetType() + " " + e.Message);
                }
              catch (Exception e) 
               {
                Console.WriteLine($"Bild {temp} not found: "+e.Message);

              } 
            }
            

        }

        public void downloadAll(string path,int maxItems) //Diese Methode wird aus Datacollection aufgerufen, löst alle DownloadTasks aus und wartet auf Beendigung
        {
            counterHolder counter = new counterHolder() { Value = maxItems };

            Task[] downloadtasks = new Task[5];

            for (int i = 0; i < downloadtasks.Length; i++)
            {
                downloadtasks[i] = DownloadFilesAsync(path,counter); //Auslösen der Tasks, übergeben wird downloadpfad und counter-Class
            }
            Task.WaitAll(downloadtasks);

        }
        public Dataset(string key, string label)
        {
            this.Label = label;
            this.Key = key;
            this.ids = new ConcurrentQueue<string>();
        }

        public override string ToString()
        {

            return ("Key: " + Key + "Label: " + Label);
        }
        public bool Equals(Dataset compare)
        {
            if (compare == null) return false;
            return (this.Key.Equals(compare.Key));
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Dataset objAsPart = obj as Dataset;
            if (objAsPart == null) return false;
            else return Equals(objAsPart);
        }
        
    }
}
