using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Tools; 

namespace Tests
{
    public class ToolsTester
    {

        ///<include file='TestsDoc/Tests.xml' path='Tests/Member[@name="TestFileNameInput"]/*'/>
        [Theory]
        [InlineData("Model", true)]
        [InlineData("Model\\", false)]
        [InlineData("Model/", false)]
        [InlineData("Model ", false)]
        public void TestFileNameInput(string ProposedFileName, bool Expected)
        { 
            Assert.Equal(Expected, ConsoleTools.FileNameInput(ProposedFileName)); 
        }
    }
}
