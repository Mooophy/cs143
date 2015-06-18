using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dragon;

namespace UnitTests
{
    [TestClass]
    public class TestSymbol
    {
        [TestMethod]
        public void TestType()
        {
            Assert.AreEqual("int", Dragon.Type.Int.Lexeme);
            Assert.AreEqual(Tag.BASIC, Dragon.Type.Int.TagValue);
            Assert.AreEqual(4, Dragon.Type.Int.Width);

            Assert.AreEqual("float", Dragon.Type.Float.Lexeme);
            Assert.AreEqual(Tag.BASIC, Dragon.Type.Float.TagValue);
            Assert.AreEqual(8, Dragon.Type.Float.Width);

            Assert.AreEqual("char", Dragon.Type.Char.Lexeme);
            Assert.AreEqual(Tag.BASIC, Dragon.Type.Char.TagValue);
            Assert.AreEqual(1, Dragon.Type.Char.Width);

            Assert.AreEqual("bool", Dragon.Type.Bool.Lexeme);
            Assert.AreEqual(Tag.BASIC, Dragon.Type.Bool.TagValue);
            Assert.AreEqual(1, Dragon.Type.Bool.Width);

            Assert.IsTrue(Dragon.Type.Numeric(Dragon.Type.Int));
            Assert.IsTrue(Dragon.Type.Numeric(Dragon.Type.Float));
            Assert.IsTrue(Dragon.Type.Numeric(Dragon.Type.Char));
            Assert.IsFalse(Dragon.Type.Numeric(Dragon.Type.Bool));
        }
    }
}
