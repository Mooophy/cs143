using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dragon;

namespace UnitTests
{
    [TestClass]
    public class TestLexer
    {
        [TestMethod]
        public void TestTag()
        {
            Assert.AreEqual(256, Tag.AND);
            Assert.AreEqual(257, Tag.BASIC);
            Assert.AreEqual(258, Tag.BREAK);
            Assert.AreEqual(259, Tag.DO);
            Assert.AreEqual(260, Tag.ELSE);
            Assert.AreEqual(261, Tag.EQ);
            Assert.AreEqual(262, Tag.FALSE);
            Assert.AreEqual(263, Tag.GE);
            Assert.AreEqual(264, Tag.ID);
            Assert.AreEqual(265, Tag.IF);
            Assert.AreEqual(266, Tag.INDEX);
            Assert.AreEqual(267, Tag.LE);
            Assert.AreEqual(268, Tag.MINUS);
            Assert.AreEqual(269, Tag.NE);
            Assert.AreEqual(270, Tag.NUM);
            Assert.AreEqual(271, Tag.OR);
            Assert.AreEqual(272, Tag.REAL);
            Assert.AreEqual(273, Tag.TEMP);
            Assert.AreEqual(274, Tag.TRUE);
            Assert.AreEqual(275, Tag.WHILE);
        }

        [TestMethod]
        public void TestToken()
        {
            //var tok = new Token(42);
            //Assert.AreEqual()
        }
    }
}
