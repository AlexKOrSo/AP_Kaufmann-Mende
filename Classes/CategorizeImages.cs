using System;
using System.Collections.Generic;
using Classes;
using System.Linq;
using System.Text;
using Tools;
using Microsoft.ML;
using System.IO;
using HTMLTools;
using System.Threading.Tasks;

namespace CategorizingImages{
    public static class ImageCategorizer{

        public static List<CategorizedImage> Categorizer(List<Image> input){
            MLContext myContext=new MLContext();
            DataViewSchema modelSchema;

            ITransformer trainedModel = myContext.Model.Load("model.zip", out modelSchema);
            List<CategorizedImage> predictions=new List<CategorizedImage>();
            foreach (Image item in input)
            {
                predictions.Add( ModelUser.ClassifySingleImg(myContext,item,trainedModel));
            }
            return predictions;
        }
        public static List<Image> Initialization(){
            
            List<Image> input=new List<Image>();
            System.Console.WriteLine("..... Bilder kategorisieren");
            while (!ConsoleTools.YesNoInput("Eigene Bilder in den Unterordner OwnImages eingefügt?"))
            {
                
            }
            
            foreach (var item in Directory.GetFiles(PathFinder.OwnImagesDir))
            {
                input.Add(new Image{Path=item});
            }
            // Parsen der Bilder, rückgabe als Image
            return input;
           
        }
    }
}