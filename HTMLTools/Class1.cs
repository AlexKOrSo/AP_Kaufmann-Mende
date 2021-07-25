using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace HTMLTools
{
    public interface IHtmldata
    {
        string GetLabel();
        string GetFilePath();
        IHtmldata GetHtmldata();
    }
    public interface IHtmlable
    {
        public IHtmldata GetHtmldata();
    }

    public static class HTMLCreator
    {
        public static string GenerateHref(IHtmldata image)
        {
            return $"<a href=\"{image.GetFilePath()}\">{image.GetLabel()}</a><br>";
        }

        public static void Result(IEnumerable<IHtmlable> images,string filepath,string filename)
        {
            StreamWriter sw = new StreamWriter(Path.Combine(filepath, filename+".html"));
            sw.WriteLine("<!DOCTYPE html>");
            sw.WriteLine("<html>");
            sw.WriteLine("<body>");
            foreach (var item in images)
            {
                sw.WriteLine(GenerateHref(item.GetHtmldata()));
            }

            sw.WriteLine("</html>");
            sw.WriteLine("</body>");
            sw.Dispose();



        }
    }
}
