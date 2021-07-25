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

            List<CategorizedImage> predictions=new List<CategorizedImage>();//Liste an CategorizedImages
            foreach (Image item in input)//für jedes element der input-Liste wird ClassifySingleImg aufgerufen, Rückgabe in predictions-Liste hinzugefügt.
            {
                predictions.Add( ModelUser.ClassifySingleImg(myContext,item,trainedModel));
            }
            return predictions; //Rückgabe der Predictions
        }
        public static List<Image> Initialization(){ //Initialisierung
            
            List<Image> input=new List<Image>(); //Liste an zu kategorisierenden Bildern
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
            //Rückgabe der Bilder
            return input;
           
        }
    }
}
