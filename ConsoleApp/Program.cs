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
            //IDataView LoadedData = mlContext.Data.ShuffleRows(mlContext.Data.LoadFromEnumerable(LoadedImages));
            //StreamWriter sw=new StreamWriter(Path.Combine(PathFinder.FindOrigin(), "test.tsv"));
            //sw.WriteLine("Label,ImageSource");
            //foreach (var item in LoadedImages)
           // {
            //    sw.WriteLine(item.Label + "," + item.Path);
           // }
            //sw.Dispose();

           

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
