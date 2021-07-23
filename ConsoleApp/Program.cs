using System;
using Tools;
using System.IO;
using MLData;
using System.Collections.Generic;
using Microsoft.ML;
using Classes;
using Microsoft.ML.Data;
using static Microsoft.ML.DataOperationsCatalog;
using Microsoft.ML.Transforms;
using Microsoft.ML.Vision;


namespace ConsoleApp
{
    class Program
    {
        public static void TrainingChoice(MLContext mlContext)
        {

            //DirectoryInfo PreviousData = Directory.CreateDirectory(assets); 
            CustomBuilder.Initialization(PathFinder.FindOrigin());
            IEnumerable<Image> LoadedImages = CustomBuilder.ImageCollector();
            IDataView LoadedData = mlContext.Data.ShuffleRows(mlContext.Data.LoadFromEnumerable(LoadedImages));


            Console.WriteLine("Making Pipeline");
            var Pipeline = mlContext.Transforms.Conversion.MapValueToKey(inputColumnName: "Label", outputColumnName: "LabelKey")
            .Append(mlContext.Transforms.LoadRawImageBytes(
            outputColumnName: "Img",
            imageFolder: PathFinder.ImageDir,
            inputColumnName: "Path"));


            IDataView TransformedData = Pipeline.Fit(LoadedData).Transform(LoadedData);
            TrainTestData trainSplit = mlContext.Data.TrainTestSplit(data: TransformedData, testFraction: 0.3);
            TrainTestData validationTestSplit = mlContext.Data.TrainTestSplit(trainSplit.TestSet);



            IDataView trainSet = trainSplit.TrainSet;
            IDataView validationSet = validationTestSplit.TrainSet;
            IDataView testSet = validationTestSplit.TestSet;


            var classifierOptions = new ImageClassificationTrainer.Options()
            {
                FeatureColumnName = "Path",
                //target variable column 
                LabelColumnName = "LabelKey",
                //IDataView containing validation set
                ValidationSet = validationSet,
                //define pretrained model to be used
                Arch = ImageClassificationTrainer.Architecture.ResnetV2101,
                //track progress during model training
                MetricsCallback = (metrics) => Console.WriteLine(metrics),

                TestOnTrainSet = false,
                //whether to use cached bottleneck values in further runs
                ReuseTrainSetBottleneckCachedValues = true,

                ReuseValidationSetBottleneckCachedValues = true
            };


            var trainingPipeline = mlContext.MulticlassClassification.Trainers.ImageClassification(classifierOptions).Append(mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));
            ITransformer trainedModel = trainingPipeline.Fit(trainSet);

            ModelUser.ClassifyMultiple(mlContext, testSet, trainedModel);



        }

        public static ITransformer RetrainPipeline(MLContext context, IDataView trainData)
        {
            var pipeline = BuildPipeline(context);
            var model = pipeline.Fit(trainData);

            return model;
        }

        public static IEstimator<ITransformer> BuildPipeline(MLContext mlContext)
        {
            // Data process configuration with pipeline data transformations //Aus MS-Doku
            var pipeline = mlContext.Transforms.Conversion.MapValueToKey(@"Label", @"Label")
                                    .Append(mlContext.Transforms.LoadRawImageBytes(@"ImageSource_featurized", @"ImageSource"))
                                    .Append(mlContext.Transforms.CopyColumns(@"Features", @"ImageSource_featurized"))
                                    .Append(mlContext.MulticlassClassification.Trainers.ImageClassification(labelColumnName: @"Label"))
                                    .Append(mlContext.Transforms.Conversion.MapKeyToValue(@"PredictedLabel", @"PredictedLabel"));

            return pipeline;
        }



        static void Main(string[] args)
        {

            MLContext myContext = new MLContext(); 
            Console.WriteLine("Before Exceptiom");
            string OriginPath = null ;
            try { 
            OriginPath = PathFinder.FindOrigin(); // sucht nach .Index-Datei, speichert deren Pfad
                }
            catch(Exception) {Console.WriteLine(@"Couldn't find .Index-File"); }
            Console.WriteLine("Willkommen in der Konsolen-App zur Bildklassifizierung auf Grundlage von Machine Learning");
            
            bool IsValidKey = false;
            char PressedKey=' '; 
            while (!IsValidKey) 
            {
                Console.WriteLine("Möchten Sie (1) Bilder kategorisieren oder (2) das Modell neu trainieren?");
                IsValidKey = ConsoleTools.IsValidKey(ref PressedKey, 1);
            }

            if (PressedKey == '1') { 
                
            } //Überleitung zur Bildklassifizierung
            else if (PressedKey == '2') {
                TrainingChoice(myContext);
            } //Überleitung zum Training

            DirectoryInfo TrainingDir=PathFinder.MakeDirectory("TrainingModel");
            Console.WriteLine(TrainingDir.FullName); 
            Exit:
            Console.WriteLine("Beende Programm");
        }
    }
}
