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

        public static List<CategorizedImage> Categorizer(List<Image> input,ITransformer trainedModel, MLContext myContext){
           // MLContext myContext=new MLContext();
            //DataViewSchema modelSchema;
            //string modelName=CustomBuilder.GetModelNames();
            //ITransformer trainedModel = myContext.Model.Load(modelName, out modelSchema);
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
                if (item.Contains(@".jpg")||item.Contains(@".png"))
                {
                    input.Add(new Image{Path=item});
                }

            }
            // Parsen der Bilder, rückgabe als Image
            return input;
           
        }
    }
}
