using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML; 

namespace Classes
{
    ///<include file='ClassesDoc/ModelUser.xml' path='ModelUser/Member[@name="ModelUser"]/*'/>
    public class ModelUser
    {
        ///<include file='ClassesDoc/ModelUser.xml' path='ModelUser/Member[@name="OutputPrediction"]/*'/>
        public static void OutputPrediction(CategorizedImage Prediction)
        {
            Console.WriteLine($"Image: {Prediction.Path} | Actual Label: {Prediction.LabeledAs} | Predicted Label: { Prediction.Path}");
        }
        ///<include file='ClassesDoc/ModelUser.xml' path='ModelUser/Member[@name="ClassifySingleImg"]/*'/>
        public static CategorizedImage ClassifySingleImg(MLContext mlContext, Image data, ITransformer trainedModel)
        {
            PredictionEngine<Image, CategorizedImage> predEngine = mlContext.Model.CreatePredictionEngine<Image, CategorizedImage>(trainedModel);
            
            CategorizedImage Prediction = predEngine.Predict(data);
            Console.WriteLine(Prediction.LabeledAs+" "+Prediction.PredictedImageLabel+" "+Prediction.Path);
            
            Console.WriteLine("Vorhersage für ein Bild");
            return Prediction;
          
        }
        ///<include file='ClassesDoc/ModelUser.xml' path='ModelUser/Member[@name="ClassifyMultiple"]/*'/>
        public static void ClassifyMultiple(MLContext mlContext, IDataView data, ITransformer trainedModel)
        {
            IDataView predictionData = trainedModel.Transform(data);
            IEnumerable<CategorizedImage> predictions = mlContext.Data.CreateEnumerable<CategorizedImage>(predictionData, reuseRowObject: true).Take(20);
            Console.WriteLine("Vorhersage für mehrere Bilder");
            foreach (var p in predictions)
            {
                OutputPrediction(p); 
            }
        }
    }
}
