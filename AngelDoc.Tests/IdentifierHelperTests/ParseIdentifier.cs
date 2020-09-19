using System.Collections.Generic;
using NUnit.Framework;

namespace AngelDoc.Tests.IdentifierHelperTests
{
    public class ParseIdentifier
    {
        private List<string> _result;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = new IdentifierHelper();
            _result = identifierHelper.ParseIdentifier("ThisIsATestIdentifier");
        }

        [Test]
        public void ResultIsCorrect()
        {
            Assert.That(_result,
                Is.EquivalentTo(new List<string> { "this", "is", "a", "test", "identifier" }));
        }
    }
}
