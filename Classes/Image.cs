using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    public class Image
    {
        public string Path;
        public string Label;
        public byte[] ByteRepresentation;
        public uint LabelKey;
        public Image(string Path, string Label)
        {
            this.Path = Path;
            this.Label = Label;
        }
        public Image() { }
    }
}
