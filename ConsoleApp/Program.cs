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
            ITransformer GeneratedModel = CustomBuilder.GenerateModel(mlContext);
            Console.WriteLine("Modell erfolgreich trainiert!\nEs wurden folgende Kategorien trainiert: ");
            foreach (string Label in TSVMaker.LabelNames) Console.WriteLine($"***{Label}");
            

        }
        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }


        static void Main(string[] args)
        {
           
            
            try
            {
                DeleteDirectory(PathFinder.ImageDir);
            }
            catch (Exception) { }

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

        Exit:
            Console.WriteLine("Beende Programm");
        }
    }
}
