using System;
using System.Collections.Generic;
using System.IO;
using Classes;
using Microsoft.ML;
using Tools;

namespace CategorizingImages
{
    ///<include file='ClassesDoc/ImageCategorizer.xml' path='ImageCategorizer/Member[@name="ImageCategorizer"]/*'/>
    public static class ImageCategorizer
    {

        ///<include file='ClassesDoc/ImageCategorizer.xml' path='ImageCategorizer/Member[@name="Categorizer"]/*'/>
        public static List<CategorizedImage> Categorizer(List<Image> input, ITransformer trainedModel, MLContext myContext)
        {

            List<CategorizedImage> predictions = new List<CategorizedImage>();
            foreach (Image item in input)
            {
                predictions.Add(ModelUser.ClassifySingleImg(myContext, item, trainedModel));
            }
            return predictions;
        }
        ///<include file='ClassesDoc/ImageCategorizer.xml' path='ImageCategorizer/Member[@name="Initialization"]/*'/>
        public static List<Image> Initialization()
        {

            List<Image> input = new List<Image>();
            System.Console.WriteLine("..... Bilder kategorisieren");
            while (!ConsoleTools.YesNoInput("Eigene Bilder in den Unterordner OwnImages eingefügt?"))
            {

            }

            string SourceDir = PathFinder.OwnImagesDir;
            Directory.CreateDirectory(SourceDir);

            int ImageCounter = 0;
            foreach (var item in Directory.GetFiles(PathFinder.OwnImagesDir))
            {
                if (item.EndsWith(@".jpg") || item.EndsWith(@".png"))
                {
                    input.Add(new Image { Path = item });
                    ImageCounter++;
                }

            }

            if (ImageCounter == 0)
            {
                Console.WriteLine($"Es befinden sich allerdings keine Bilder im Ordner {SourceDir}!\nProgrammabbruch!");
                throw new FileNotFoundException(null);
            }

            return input;

        }
    }
}
