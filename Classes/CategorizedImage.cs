using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    
    public class CategorizedImage : Image
    {
        public string PredictedImageLabel { get; private set; }

        public float[] Score; 

        public CategorizedImage(string LabeledAs)
        {
            this.LabeledAs = LabeledAs;
            
            

            HTMLTools.ProcessedImages.Add(this); 
        }
        public CategorizedImage() { }
    }
}
