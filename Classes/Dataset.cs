using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Tools;
using Amazon.S3.Model;
using Amazon.Runtime;
using System.Net.NetworkInformation;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace MLData
{
    public class Dataset: IEquatable<Dataset> 
    {
        public string Label { get; set; } //Bezeichnung
        public string Key { get; set; } //Schlüssel, der in Datenbank hinterlegt ist
        public ConcurrentQueue<string> ids; //Asynchrone Queue

        public class counterHolder
        {
            public int Value;
        }

        async Task DownloadFilesAsync(string path, counterHolder counter)
        {
            
            string bucketName = "open-images-dataset";
            string filename;

            Console.WriteLine(ids.Count.ToString());
            RegionEndpoint reg = RegionEndpoint.USEast1;
            AmazonS3Client s3Client = null;
            int timeout=30;
            try
            {
                s3Client = new AmazonS3Client(null, new AmazonS3Config() { RegionEndpoint = reg, Timeout = TimeSpan.FromSeconds(timeout) });
            }
            catch (AmazonClientException)
            {
                Console.WriteLine("AWS-Config-Fehler");
                throw;
            }
            Console.WriteLine($"Download wird gestartet, Timeout beträgt {timeout} Sekunden");
            var fileTransferUtility = new TransferUtility(s3Client);

            string temp;
            while (ids.TryDequeue(out temp))
            {

                filename = Path.Combine(path,(temp +".jpg"));
                Console.WriteLine($"Bilder in Queue: {ids.Count.ToString()}");

              
               try{

                    
                    await fileTransferUtility.DownloadAsync(filename, bucketName, "train/" + temp + ".jpg");
                    int currentCounter=Interlocked.Decrement(ref counter.Value);
                    System.Console.WriteLine($"Zahl herunterzuladender Bilder: {currentCounter}");
                    if (currentCounter<=0)
                    {
                        break;
                    }

                }
              catch (Exception)
               {
                Console.WriteLine($"Bild {temp} not found");

              }
                //await Task.Delay(100);
                
            }
            

        }

        public void downloadAll(string path,int maxItems)
        {
            counterHolder counter = new counterHolder() { Value = maxItems };

            //getIDs(ref ids);
            try
            {
                if (CheckNetwork.PingAWS() == false)
                {
                    Console.WriteLine("Netzwerkfehler");
                    throw new Exception("Kontakt zum ENDPOINT USEAST1 nicht moeglich.");
                }
            }
            catch (Exception)
            {

                throw;
            }
            Task[] downloadtasks = new Task[5];

            for (int i = 0; i < downloadtasks.Length; i++)
            {
                downloadtasks[i] = DownloadFilesAsync(path,counter);
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
