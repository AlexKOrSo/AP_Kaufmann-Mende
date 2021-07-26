using System;
using Xunit;
using Classes;
using Tools;
using Microsoft.ML;
using MLData;
using System.IO;
using Microsoft.ML.Data;

namespace Tests
{
    public class CustomModelBuilderTest
    {
        [Theory]
        [InlineData(null, null)] //Fehlerhaftes Modell übergeben
        public void TestGenerateModel(MLContext mlContext, ITransformer Expected)
        {
            Assert.Equal(Expected, CustomBuilder.GenerateModel(mlContext)); 
        }

        [Theory]
        [InlineData(true, true)] //Korrekter Pfad auf .Info-Datei
        [InlineData(false, true)] //Falscher Pfad
        public void TestAddModelInfo(bool PathIsValid, bool Expected)
        {
            TSVMaker.LabelNames =new string[]{ "Label1", "Label2"}; 
            string TestPath = null;
            if (PathIsValid) TestPath = PathFinder.ModelDir;
            else TestPath = "C:\\Users\\mender\\source";
            TestPath = Path.Combine(TestPath, "Model.model");

            Assert.Equal(Expected, CustomBuilder.AddModelInfo(TestPath)); 
            //Methode schreibt sowohl in Richtigen, als auch in Falschen Ordner => Zulässiges Verhalten, da Methode der korrekte Pfad vorgegeben wird
        }

        
    }
}
