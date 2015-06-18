using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dragon;

namespace UnitTests
{
    [TestClass]
    public class TestNode
    {
        [TestMethod]
        public void TestNodeClass()
        {
            var node = new Node();
            Assert.AreEqual(1, node.NewLable());

            node.EmitLabel(42);
            node.Emit("some_OpCode"); //check this from output
        }

        [TestMethod]
        public void TestExpr()
        {
            var expr = new Expr(new Num(42), Dragon.Type.Int);
            Assert.AreEqual(Tag.NUM, expr.Op.TagValue);
            Assert.AreEqual(Dragon.Type.Int, expr.Type);

            Assert.AreSame(expr, expr.Gen());
            Assert.AreSame(expr, expr.Reduce());

            expr.EmitJumps("i < 0", 10, 20);//check from output
            Console.WriteLine();
            expr.Jumping(10, 20);
        }
    }
}
