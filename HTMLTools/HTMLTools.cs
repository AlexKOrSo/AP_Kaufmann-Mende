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

            string htmlPath = Path.Combine(filepath, filename + ".html"); 
            try
            {
                using (StreamWriter sw = new StreamWriter(htmlPath))
                {
                    sw.WriteLine("<!DOCTYPE html>");
                    sw.WriteLine("<html>");
                    sw.WriteLine("<body>");
                    foreach (var item in images)
                    {
                        sw.WriteLine(GenerateHref(item.GetHtmldata()));
                    }

                    sw.WriteLine("</body>");
                    sw.WriteLine("</html>");
                    sw.Dispose();
                    Console.WriteLine($"Ergebnis-Datei unter {htmlPath} gespeichert!"); 
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"{htmlPath} konnte nicht geschrieben werden"); 
            }

        }
    }
}
