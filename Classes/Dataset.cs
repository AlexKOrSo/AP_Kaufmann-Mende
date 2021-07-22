using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace MLData
{
    public class Dataset: IEquatable<Dataset> 
    {
        public string Label { get; set; } //Bezeichnung
        public string Key { get; set; } //Schlüssel, der in Datenbank hinterlegt ist
        public ConcurrentQueue<string> ids; //Asynchrone Queue

        async Task DownloadFilesAsync(string path)
        {
            string bucketName = "open-images-dataset";
            string filename;
            Console.WriteLine(ids.Count.ToString());
            RegionEndpoint reg = RegionEndpoint.USEast1; 
            AmazonS3Client s3Client = new AmazonS3Client(null, new AmazonS3Config() { RegionEndpoint = reg, Timeout = TimeSpan.FromSeconds(180) });
            Console.WriteLine("Download wird gestartet, Timeout beträgt 180 Sekunden.");
            var fileTransferUtility = new TransferUtility(s3Client);

            string temp;
            while (ids.TryDequeue(out temp))
            {

                filename = path + @"\" + temp + ".jpg";
                Console.WriteLine(ids.Count.ToString());

                try
                {
                    await fileTransferUtility.DownloadAsync(filename, bucketName, "train/" + temp + ".jpg");
                }
                catch (Exception)
                {
                    Console.WriteLine($"{temp} not found");

                }
                //await Task.Delay(100);
                
            }
            

        }

        public void downloadAll(string path)
        {

            //getIDs(ref ids);

            Task[] downloadtasks = new Task[5];

            for (int i = 0; i < downloadtasks.Length; i++)
            {
                downloadtasks[i] = DownloadFilesAsync(path);
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
