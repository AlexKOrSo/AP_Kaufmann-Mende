using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using MLData;
using Tools;
using System.IO;
using Microsoft.ML.Data;

namespace Classes
{
    public class CustomBuilder
    {
        public static List<Dataset> Labels { get; private set; }
        public static DataCollection Data { get; private set; } 
        static CustomBuilder()
        {
            Labels = new List<Dataset>();
            
        }
        public static void Initialization(string path)
        {
            {

                try
                {
                    
                    bool run = true;
                    Data = new DataCollection(path, 500);
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
                    TSVMaker.LogAllData(PathFinder.ImageDir, Data.Labels); 
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine("Alles auf Anfang");
                }
            }
        }

        public static ITransformer GenerateModel(MLContext mlContext)
        {


            string ModelFolder = PathFinder.ModelDir;  
            string ModelLocation= Path.Combine(ModelFolder, "tensorflow_inception_graph.pb");

            string TrainingTags = Path.Combine(PathFinder.ImageDir, TSVMaker.TrainData);
            string TestTags = Path.Combine(PathFinder.ImageDir, TSVMaker.TestData); 
            Console.WriteLine(nameof(Image.Path));

            //Tranformationen der Eingaben für nachfolgende Verarbeitungsschritte
            IEstimator<ITransformer> pipeline = mlContext.Transforms.LoadImages(outputColumnName: "input", imageFolder: null, inputColumnName: nameof(Image.Path)) //_imagesFolder
                                                                                                                                                                            // The image transforms transform the images into the model's expected format.
                            .Append(mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: InceptionSettings.ImageWidth, imageHeight: InceptionSettings.ImageHeight, inputColumnName: "input"))
                            .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: InceptionSettings.ChannelsLast, offsetImage: InceptionSettings.Mean))
                            .Append(mlContext.Model.LoadTensorFlowModel(ModelLocation).
                            ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2_pre_activation" }, inputColumnNames: new[] { "input" }, addBatchDimensionInput: true))
                            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey", inputColumnName: "LabeledAs"))
                            .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "softmax2_pre_activation"))
                            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedImageLabel", "PredictedLabel"))
                            .AppendCacheCheckpoint(mlContext);


            
            IDataView TrainingData = mlContext.Data.LoadFromTextFile<Image>(path: TrainingTags, separatorChar: ';');

            Console.WriteLine("Training Gestartet\nDies kann je nach Anzahl der Bilder einige Zeit dauern!");
            ITransformer TrainedModel = pipeline.Fit(TrainingData);

            Console.WriteLine("Trainiertes Modell testen"); 
            IDataView TestData = mlContext.Data.LoadFromTextFile<Image>(path: TestTags, separatorChar: ';');
            IDataView TestPredictions = TrainedModel.Transform(TestData);

            IEnumerable<CategorizedImage> ImagePredictionData = mlContext.Data.CreateEnumerable<CategorizedImage>(TestPredictions, true);
            DisplayResults(ImagePredictionData);

            Console.WriteLine("Statistiken zum Training: ");
            MulticlassClassificationMetrics metrics =
                mlContext.MulticlassClassification.Evaluate(TestPredictions,
                  labelColumnName: "LabelKey",
                  predictedLabelColumnName: "PredictedImageLabel");
            
            Console.WriteLine($"LogLoss: {metrics.LogLoss}");
            Console.WriteLine($"PerClassLogLoss: {String.Join(" , ", metrics.PerClassLogLoss.Select(c => c.ToString()))}");

            Console.WriteLine("Sie können das Modell jetzt speichern. Unter welchem Namen sol das Modell gespeichert werden? (Ohne Extension)");
            bool CorrectName = false;
            bool FileExists = true; 
            string Input = ""; 
            do
            {
                Input = Console.ReadLine();
                CorrectName = ConsoleTools.FileNameInput(Input);
                FileExists = File.Exists(Path.Combine(ModelFolder, Input + ".model")) ? true : false;
                if (FileExists) Console.WriteLine("File existiert schon, bitte neuen Namen ausdenken"); 
            }
            while(!CorrectName || FileExists);

            string ModelName = Input + ".model"; 
            string NewModelPath = Path.Combine(ModelFolder, ModelName); 
            mlContext.Model.Save(TrainedModel, TrainingData.Schema, NewModelPath);
            Console.WriteLine($"Das Modell ist unter {NewModelPath} gespeichert");
            AddModelInfo(NewModelPath); 
            

            return TrainedModel;
        }

        /*public static bool RetrieveModelInfo(string ModelPath) ***Wahrscheinlich obsolet***
        {
            string ModelName = Path.GetFileName(ModelPath); 
            string line = "";
            List<string> Lines = new List<string>(); 
            using (StreamReader sr = new StreamReader(Path.Combine(PathFinder.ModelDir,".Info")))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    Lines.Add(line); 
                }
            }
            foreach(var Line in Lines)
            {
                string[] Parts = Line.Split(';'); 
                if(Equals(Parts[0], ModelName))
                {
                    Console.WriteLine($"Modell {ModelName} mit Trainierten Klassen {Parts[1]} gefunden!");
                    return true; 
                } 
            }
            return false; 


        }
        */
        private static bool AddModelInfo(string ModelPath)
        {
            string ModelName = Path.GetFileName(ModelPath);
            string SingleLabels = "";
            foreach (var SingleLabel in TSVMaker.LabelNames) SingleLabels += SingleLabel + "-";
            try
            {
                using (StreamWriter sw = File.AppendText(Path.Combine(PathFinder.ModelDir, ".Info")))
                {
                    sw.WriteLine(ModelName + ';' + SingleLabels);
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Speichern des Modells nicht möglich. Existiert Zugriff auf {Path.Combine(PathFinder.ModelDir, ".Info")}?");
                return false; 
            }
            return true; 
        }
        public static string GetModelNames()
        {
            string line = null;
            List<string> ModelNames = new List<string>();
            Console.WriteLine("Folgende Modelle sind vorhanden:\n "); 
            using (StreamReader sr = new StreamReader(Path.Combine(PathFinder.ModelDir, ".Info")))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] Parts = line.Split(';');
                    ModelNames.Add(Parts[0]);
                    Console.WriteLine($"Modell: **{Parts[0].Split('.')[0]}** mit den trainierten Kategorien: **{Parts[1]} (Nr. {ModelNames.IndexOf(Parts[0]) })**");
                }
            }

            Console.WriteLine("Bitte Entscheidung für ein Modell durch Eingabe der jeweiligen Nummer treffen und mit Enter bestätigen\nEingabe muss wiederholt werden, wenn inkorrekt"); 
            bool IsNumber = false;
            int Choice=-1;
            bool IsValidNumber = false;
            while (!IsNumber || !IsValidNumber)
            {
                IsNumber = int.TryParse(Console.ReadLine(), out Choice);
                IsValidNumber = (Choice >= 0 && Choice < ModelNames.Count) ? true : false;
            }


            Console.WriteLine($"Sie haben sich für {ModelNames[Choice].Split('.')[0]} entschieden"); 
            return ModelNames[Choice]; 


        }
            

        private static void DisplayResults(IEnumerable<CategorizedImage> PredictedData)
        {
            foreach (CategorizedImage Result in PredictedData)
            {

                string Category = null ; 
                for (int i=0; i < TSVMaker.LabelNames.Length; i++)
                { 
                    if (Result.Score.Max() == Result.Score[i]) Category = TSVMaker.LabelNames[i]; 
                }

                Console.WriteLine($"Bild: {Path.GetFileName(Result.Path)} Gelabelt Als: {Result.GetLabelFromPath()} Bestimmt Als: {Category} Sicherheit: {Result.Score.Max()} ");
            }
           
        }

        private struct InceptionSettings
        {
            public const int ImageHeight = 224;
            public const int ImageWidth = 224;
            public const float Mean = 117;
            public const float Scale = 1;
            public const bool ChannelsLast = true;
        }

    }
}
