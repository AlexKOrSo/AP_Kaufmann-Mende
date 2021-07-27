using System.Collections.Generic;
using MLData;
using Tools;
using Xunit;

namespace Tests
{
    public class DataCollectionTester
    {
        DataCollection Data = new DataCollection(PathFinder.FindOrigin(), 25); //Keinen parameterfreien Konstruktor nur zum Testen eingefügt, daher mit willkürlichen Werten initialisiert

        ///<include file='TestsDoc/Tests.xml' path='Tests/Member[@name="TestEmptyFindLables"]/*'/>
        [Fact]
        public void TestEmptyFindLables()
        {

            Assert.Empty(Data.FindLables("blablabla"));
        }

        ///<include file='TestsDoc/Tests.xml' path='Tests/Member[@name="TestFindLables"]/*'/>
        [Theory]
        [InlineData("Jaguar", 33)]
        [InlineData("Car", 306)]
        public void TestFindLables(string SearchKey, int ExpectedListCount)
        {

            List<Dataset> TestSet = Data.FindLables(SearchKey);
            Assert.Equal(ExpectedListCount, TestSet.Count);
        }
    }
}
