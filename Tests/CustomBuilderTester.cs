using System.IO;
using Classes;
using Microsoft.ML;
using Tools;
using Xunit;

namespace Tests
{
    public class CustomModelBuilderTest
    {
        ///<include file='TestsDoc/Tests.xml' path='Tests/Member[@name="TestGenerateModel"]/*'/>
        [Theory]
        [InlineData(null, null)] //Fehlerhaftes Modell �bergeben
        public void TestGenerateModel(MLContext mlContext, ITransformer Expected)
        {
            Assert.Equal(Expected, CustomBuilder.GenerateModel(mlContext));
        }

        ///<include file='TestsDoc/Tests.xml' path='Tests/Member[@name="TestAddModelInfo"]/*'/>
        [Theory]
        [InlineData(true, true)] //Korrekter Pfad auf .Info-Datei
        [InlineData(false, true)] //Falscher Pfad, trotdem Indeizierung m�glich
        public void TestAddModelInfo(bool PathIsValid, bool Expected)
        {
            TSVMaker.LabelNames = new string[] { "Label1", "Label2" };
            string TestPath = null;
            if (PathIsValid) TestPath = PathFinder.ModelDir;
            else TestPath = "C:\\Users\\mender\\source";
            TestPath = Path.Combine(TestPath, "Model.model");

            Assert.Equal(Expected, CustomBuilder.AddModelInfo(TestPath));
            //Methode schreibt sowohl in Richtigen, als auch in Falschen Ordner => Zul�ssiges Verhalten, da Methode der korrekte Pfad vorgegeben wird
        }


    }
}
