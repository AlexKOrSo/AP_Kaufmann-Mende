using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;


namespace Classes
{
    public class Image
    {
        [LoadColumn(0)]
        public string Path;

        [LoadColumn(1)]
        public string LabeledAs;

        public Image(string Path, string Label)
        {
            this.Path = Path;
            this.LabeledAs = Label;
        }

        public Image() { }
    }
}
