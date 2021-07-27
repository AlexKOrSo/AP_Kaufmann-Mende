using System;
using System.Collections.Generic;
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

    ///<include file='HTMLDoc/HTMLTools.xml' path='HTMLTools/Member[@name="HTMLCreator"]/*'/>
    public static class HTMLCreator
    {
        ///<include file='HTMLDoc/HTMLTools.xml' path='HTMLTools/Member[@name="GenerateHref"]/*'/>
        public static string GenerateHref(IHtmldata image)
        {
            string[] subpath = image.GetFilePath().Split(Path.DirectorySeparatorChar);
            return $"<a href=\"{image.GetFilePath()}\">{subpath[subpath.Length - 1]}: {image.GetLabel()} </a><br>";
        }
        ///<include file='HTMLDoc/HTMLTools.xml' path='HTMLTools/Member[@name="Result"]/*'/>
        public static void Result(IEnumerable<IHtmlable> images, string filepath, string filename)
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
