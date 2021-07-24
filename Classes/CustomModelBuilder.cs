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
                    TSVMaker.LogAllData(PathFinder.ImageDir, Data); 
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

        public static ITransformer GenerateModel(MLContext mlContext)
        {
            string ModelLocation=Path.Combine(PathFinder.FindOrigin(), "Classes", "Model", "tensorflow_inception_graph.pb");
            string TrainingTags = Path.Combine(PathFinder.ImageDir, "TrainingData.tsv");
            string TestTags = Path.Combine(PathFinder.ImageDir, "TestData.tsv"); 
            Console.WriteLine(nameof(Image.Path));
            // <SnippetImageTransforms>

            IEstimator<ITransformer> pipeline = mlContext.Transforms.LoadImages(outputColumnName: "input", imageFolder: null, inputColumnName: nameof(Image.Path)) //_imagesFolder
                                                                                                                                                                            // The image transforms transform the images into the model's expected format.
                            .Append(mlContext.Transforms.ResizeImages(outputColumnName: "input", imageWidth: InceptionSettings.ImageWidth, imageHeight: InceptionSettings.ImageHeight, inputColumnName: "input"))
                            .Append(mlContext.Transforms.ExtractPixels(outputColumnName: "input", interleavePixelColors: InceptionSettings.ChannelsLast, offsetImage: InceptionSettings.Mean))
                            // </SnippetImageTransforms>
                            // The ScoreTensorFlowModel transform scores the TensorFlow model and allows communication
                            // <SnippetScoreTensorFlowModel>
                            .Append(mlContext.Model.LoadTensorFlowModel(ModelLocation).
                                ScoreTensorFlowModel(outputColumnNames: new[] { "softmax2_pre_activation" }, inputColumnNames: new[] { "input" }, addBatchDimensionInput: true))
                            // </SnippetScoreTensorFlowModel>
                            // <SnippetMapValueToKey>
                            .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "LabelKey", inputColumnName: "Label"))
                            // </SnippetMapValueToKey>
                            // <SnippetAddTrainer>
                            .Append(mlContext.MulticlassClassification.Trainers.LbfgsMaximumEntropy(labelColumnName: "LabelKey", featureColumnName: "softmax2_pre_activation"))
                            // </SnippetAddTrainer>
                            // <SnippetMapKeyToValue>
                            .Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabelValue", "PredictedLabel"))
                            .AppendCacheCheckpoint(mlContext);
            // </SnippetMapKeyToValue>

            // <SnippetLoadData>
            IDataView trainingData = mlContext.Data.LoadFromTextFile<Image>(path: TrainingTags, hasHeader: true, separatorChar: ';');
            // </SnippetLoadData>

            // Train the model
            Console.WriteLine("=============== Training classification model ===============");
            // Create and train the model
            // <SnippetTrainModel>
            ITransformer model = pipeline.Fit(trainingData);
            // </SnippetTrainModel>

            // Generate predictions from the test data, to be evaluated
            // <SnippetLoadAndTransformTestData>
            IDataView testData = mlContext.Data.LoadFromTextFile<Image>(path: TestTags, hasHeader: true, separatorChar: ';');
            IDataView predictions = model.Transform(testData);

            // Create an IEnumerable for the predictions for displaying results
            IEnumerable<CategorizedImage> imagePredictionData = mlContext.Data.CreateEnumerable<CategorizedImage>(predictions, true);
            DisplayResults(imagePredictionData);
            // </SnippetLoadAndTransformTestData>

            // Get performance metrics on the model using training data
            Console.WriteLine("=============== Classification metrics ===============");

            // <SnippetEvaluate>
            MulticlassClassificationMetrics metrics =
                mlContext.MulticlassClassification.Evaluate(predictions,
                  labelColumnName: "LabelKey",
                  predictedLabelColumnName: "PredictedLabel");
            // </SnippetEvaluate>

            //<SnippetDisplayMetrics>
            Console.WriteLine($"LogLoss is: {metrics.LogLoss}");
            Console.WriteLine($"PerClassLogLoss is: {String.Join(" , ", metrics.PerClassLogLoss.Select(c => c.ToString()))}");
            //</SnippetDisplayMetrics>

            // <SnippetReturnModel>
            return model;
            // </SnippetReturnModel>
        }

        private static void DisplayResults(IEnumerable<CategorizedImage> imagePredictionData)
        {
            // <SnippetDisplayPredictions>
            foreach (CategorizedImage prediction in imagePredictionData)
            {
                Console.WriteLine($"Image: {Path.GetFileName(prediction.Path)} predicted as: {prediction.PredictedLabel} with score: {prediction.Score.Max()} ");
            }
            // </SnippetDisplayPredictions>
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
