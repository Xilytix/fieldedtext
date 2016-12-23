// Project: Xilytix.FieldedText
// Licence: Public Domain
// Web Home Page: http://www.xilytix.com/FieldedTextComponent.html
// Initial Developer: Paul Klink (http://paul.klink.id.au)

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Xilytix.FieldedText.UnitTest
{
    [TestClass]
    public class InternalTestClass : BaseTestClass
    {
        public InternalTestClass(): base("") { }

        [TestMethod]
        public void StaticTest()
        {
            Test.InternalTest.StaticTest();
        }
    }
}
