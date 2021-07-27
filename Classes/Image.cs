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
    ///<include file='ClassesDoc/Image.xml' path='Image/Member[@name="Image"]/*'/>
    public class Image
    {
        
        [LoadColumn(0)]
        ///<include file='ClassesDoc/Image.xml' path='Image/Member[@name="Path"]/*'/>
        public string Path;

        [LoadColumn(1)]
        ///<include file='ClassesDoc/Image.xml' path='Image/Member[@name="LabeledAs"]/*'/>
        public string LabeledAs;

        public Image(string Path, string Label)
        {
            this.Path = Path;
            this.LabeledAs = Label;
        }

        public Image() { }
        ///<include file='ClassesDoc' path='Image/Member[@name="GetLabelFromPath"]/*'/>
        public string GetLabelFromPath()
        {
            string Category = null;
            
            DirectoryInfo Dir=Directory.GetParent(this.Path);
            DirectoryInfo ParentDir = Directory.GetParent(Dir.FullName);
           
            string TempCategory = System.IO.Path.GetRelativePath(ParentDir.FullName, Dir.FullName);
            if (TSVMaker.LabelNames.Contains(TempCategory)) Category = TempCategory; 
            return Category; 
        }
    }
}
