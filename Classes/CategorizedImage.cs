using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HTMLTools;
namespace Classes
{
    
    public class CategorizedImage : Image, IHtmlable, IHtmldata
    {
        public string PredictedImageLabel; 
        
        public float[] Score;

        public string GetLabel() => PredictedImageLabel;
        public string GetFilePath() => Path;
        public IHtmldata GetHtmldata()=> this;
        public CategorizedImage(string LabeledAs)
        {
            this.LabeledAs = LabeledAs;
            
            

            HTMLTools.ProcessedImages.Add(this); 
        }
        public CategorizedImage() { }
    }



    
}
