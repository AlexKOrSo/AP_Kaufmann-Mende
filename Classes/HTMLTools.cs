using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public static class HTMLTools
    {
        public static List<CategorizedImage> ProcessedImages; 
        static HTMLTools()
        {
            ProcessedImages = new List<CategorizedImage>(); 
        }
        public static void MakeResultsFile(string Path)
        {
             //Schreibt HTML-Files für jedes Bild in ProcessedImages
        }
    }
}
