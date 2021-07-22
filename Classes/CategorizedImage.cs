using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class CategorizedImage
    {
        public string LabeledAs { get; private set; }
        public string Path { get; private set; }
        public string FileName {get; private set; }

        public CategorizedImage(string LabeledAs, string Path, string FileName)
        {
            this.LabeledAs = LabeledAs;
            this.Path = Path;
            this.FileName = FileName;

            HTMLTools.ProcessedImages.Add(this); 
        }
    }
}
