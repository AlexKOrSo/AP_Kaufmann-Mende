using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.IO;
using Tools; 


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
        public string GetLabelFromPath()
        {
            string Category = null;
            //Gibt Label eines Bildes zurück
            DirectoryInfo Dir=Directory.GetParent(this.Path);
            DirectoryInfo ParentDir = Directory.GetParent(Dir.FullName);
            //
            string TempCategory = System.IO.Path.GetRelativePath(ParentDir.FullName, Dir.FullName);
            if (TSVMaker.LabelNames.Contains(TempCategory)) Category = TempCategory; 
            return Category; 
        }
    }
}
