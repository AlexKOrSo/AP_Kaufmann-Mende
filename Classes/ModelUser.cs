﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML; 

namespace Classes
{
    public class ModelUser
    {
        public static void OutputPrediction(CategorizedImage Prediction)
        {
            Console.WriteLine($"Image: {Prediction.FileName} | Actual Label: {Prediction.LabeledAs} | Predicted Label: { Prediction.Path}");
        }

        public static void ClassifySingleImg(MLContext mlContext, IDataView data, ITransformer trainedModel)
        {
            PredictionEngine<Image, CategorizedImage> predEngine = mlContext.Model.CreatePredictionEngine<Image, CategorizedImage>(trainedModel);
            Image image = mlContext.Data.CreateEnumerable<Image>(data, reuseRowObject: true).First();
            CategorizedImage Prediction = predEngine.Predict(image);
            //print predicted value
            Console.WriteLine("Prediction for single image");
            OutputPrediction(Prediction);
        }

        public static void ClassifyMultiple(MLContext myContext, IDataView data, ITransformer trainedModel)
        {
            IDataView predictionData = trainedModel.Transform(data);
            IEnumerable<CategorizedImage> predictions = myContext.Data.CreateEnumerable<CategorizedImage>(predictionData, reuseRowObject: true).Take(20); //20 images
            Console.WriteLine("Prediction for multiple images");
            foreach (var p in predictions)
            {
                OutputPrediction(p); //print predicted value of each image
            }
        }
    }
}
