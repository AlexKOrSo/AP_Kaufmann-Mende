using System;
using Classes;
using Tools;
namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello World!");
            PlaceHolder.PrintPlaceHolder(); //Check for succesful reference
            Console.WriteLine(ConsoleTools.YesNoInput("Ja oder nein?"));
        }
    }
}
