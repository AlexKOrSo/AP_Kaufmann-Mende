using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace MLData
{
    ///<include file='ClassesDoc/Dataset.xml' path='Dataset/Member[@name="Dataset"]/*'/>
    public class Dataset : IEquatable<Dataset>
    {
        ///<include file='ClassesDoc/Dataset.xml' path='Dataset/Member[@name="Label"]/*'/>
        public string Label { get; set; }
        ///<include file='ClassesDoc/Dataset.xml' path='Dataset/Member[@name="Key"]/*'/>
        public string Key { get; set; }
        ///<include file='ClassesDoc/Dataset.xml' path='Dataset/Member[@name="ids"]/*'/>
        public ConcurrentQueue<string> ids;
        ///<include file='ClassesDoc/Dataset.xml' path='Dataset/Member[@name="counterHolder"]/*'/>
        public class counterHolder
        {
            public int Value;
        }
        ///<include file='ClassesDoc/Dataset.xml' path='Dataset/Member[@name="DownloadFilesAsync"]/*'/>
        async Task DownloadFilesAsync(string path, counterHolder counter)
        {

            string bucketName = "open-images-dataset";
            string filename;

            RegionEndpoint reg = RegionEndpoint.USEast1;
            AmazonS3Client s3Client = null;
            int timeout = 30;
            try
            {
                s3Client = new AmazonS3Client(null, new AmazonS3Config() { RegionEndpoint = reg, Timeout = TimeSpan.FromSeconds(timeout) });

            }
            catch (Exception e)
            {
                throw new Exception("AWS - Config - Fehler: " + e.Message);
            }
            Console.WriteLine($"Download wird gestartet, Timeout beträgt {timeout} Sekunden");
            var fileTransferUtility = new TransferUtility(s3Client);

            string temp;
            while (ids.TryDequeue(out temp))
            {

                filename = Path.Combine(path, (temp + ".jpg"));
                Console.WriteLine($"Bilder in Queue: {ids.Count.ToString()}");


                try
                {


                    await fileTransferUtility.DownloadAsync(filename, bucketName, "train/" + temp + ".jpg");
                    int currentCounter = Interlocked.Decrement(ref counter.Value);
                    System.Console.WriteLine($"Zahl herunterzuladender Bilder: {currentCounter}");
                    if (currentCounter <= 0)
                    {
                        break;
                    }

                }
                catch (Amazon.S3.AmazonS3Exception e)
                {
                    Console.WriteLine($"Bild {temp} not found: " + e.Message);
                }

                catch (Amazon.Runtime.AmazonServiceException e)
                {
                    Console.WriteLine(e.Message + " " + e.GetType());
                    throw new Exception($"Schwerwiegender Fehler in {nameof(DownloadFilesAsync)}: " + e.GetType() + " " + e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Bild {temp} not found: " + e.Message);

                }
            }


        }
        ///<include file='ClassesDoc/Dataset.xml' path='Dataset/Member[@name="downloadAll"]/*'/>
        public void downloadAll(string path, int maxItems)
        {
            counterHolder counter = new counterHolder() { Value = maxItems };

            Task[] downloadtasks = new Task[5];

            for (int i = 0; i < downloadtasks.Length; i++)
            {
                downloadtasks[i] = DownloadFilesAsync(path, counter);
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
        ///<include file='ClassesDoc/Dataset.xml' path='Dataset/Member[@name="CustomEquals"]/*'/>
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
