using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentionGeneratorTests
{
    public class GenerateClassDocs
    {
        private string _result;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = Substitute.For<IIdentifierHelper>();
            identifierHelper.ParseIdentifier("Test").Returns(new List<string> { "test" });
            var documentationGenerator = new DocumentionGenerator(identifierHelper);

            var classDef = TestHelpers.GetSyntaxSymbol<ClassDeclarationSyntax>(
@"class Test : ITest
{
}");

            _result = documentationGenerator.GenerateClassDocs(classDef);
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
