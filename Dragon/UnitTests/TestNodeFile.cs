using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dragon;

namespace UnitTests
{
    [TestClass]
    public class TestNodeFile
    {
        [TestMethod]
        public void TestNode()
        {
            var node = new Node();
            Assert.AreEqual(1, node.NewLable());

            node.EmitLabel(42);
            node.Emit("some_OpCode"); //check this from output
        }
    }
}
