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
        [Theory]
        [InlineData("Model", true)]
        [InlineData("Model\\", false)]
        [InlineData("Model/", false)]
        [InlineData("Model ", false)]

        public void TestFileNameinput(string ProposedFileName, bool Answer)
        {//Testet, ob ein String als Modell-Name taugt 
            Assert.Equal(Answer, ConsoleTools.FileNameInput(ProposedFileName)); 
        }
    }
}
