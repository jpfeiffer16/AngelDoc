using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentionGeneratorTests
{
    public class GenerateClassDocs
    {
        private DocumentionGenerator _documentationGenerator;
        private string _result;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = Substitute.For<IIdentifierHelper>();
            identifierHelper.ParseIdentifier("Test").Returns(new List<string> { "test" });
            _documentationGenerator = new DocumentionGenerator(identifierHelper);

            var classDef = TestHelpers.GetSyntaxSymbol<ClassDeclarationSyntax>(
@"class Test : ITest
{
}");

            _result = _documentationGenerator.GenerateClassDocs(classDef);
        }

        [Test]
        public void ResultIsCorrect()
        {
            Assert.That(_result, Is.EqualTo(
@"/// <summary>
/// Test.
/// </summary>
/// <seealso cref=""ITest"" />"));
        }
    }
}
