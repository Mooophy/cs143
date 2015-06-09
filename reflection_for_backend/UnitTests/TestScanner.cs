using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace UnitTests
{
    [TestClass]
    public class TestScanner
    {
        [TestMethod]
        public void TestScannerCtor()
        {
            TextReader input = File.OpenText("helloworld.gfn");
            var scanner = new Scanner(input);
            string expect = "[var][x][System.Object][hello world!][System.Object][print][x][System.Object]";
            Assert.AreEqual(expect, scanner.ToString());
        }
    }
}
