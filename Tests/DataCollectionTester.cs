using System;
using Xunit; 
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MLData;
using Tools; 

namespace Tests
{
    public class DataCollectionTester
    {
        DataCollection Data = new DataCollection(PathFinder.FindOrigin(), 25); //Keinen parameterfreien Konstruktor nur zum Testen eingefügt, daher mit willkürlichen Werten initialisiert
        [Fact]
        public void TestEmptyFindLables()
        {
            //Testet die Rückgabe bei Suchstring, der keine Übereinstimmung mit den hinterlegten Labels der Tabelle besitzt 
            Assert.Empty(Data.FindLables("blablabla")); 
        }

        [Theory]
        [InlineData("Jaguar", 33)]
        [InlineData("Car", 306)]
        public void TestFindLables(string SearchKey, int ExpectedListCount)
        {
            //Testet die Anzahl der gefundenen, passenden Labels. ExpectedListCount wurde durch manuelle Prüfung der Tabelle erhalten
            List<Dataset> TestSet = Data.FindLables(SearchKey);
            Assert.Equal(ExpectedListCount, TestSet.Count); 
        }
    }
}
