using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentionGeneratorTests
{
    public class GenerateConstructorDocs
    {
        private string _result;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = Substitute.For<IIdentifierHelper>();
            identifierHelper.ParseIdentifier("Test").Returns(new List<string> { "test" });
            identifierHelper.ParseIdentifier("testAmount").Returns(new List<string> { "test", "amount" });
            var documentationGenerator = new DocumentionGenerator(identifierHelper);

            var ctorDef = TestHelpers.GetSyntaxSymbol<ConstructorDeclarationSyntax>(
@"class Test
{
    public Test(double testAmount)
    {
    }
}", 2);

            _result = documentationGenerator.GenerateConstructorDocs(ctorDef);
        }

        [Test]
        public void ResultIsCorrect()
        {
            Assert.That(_result, Is.EqualTo(
@"/// <summary>
/// Initializes a new instance of the <see cref=""Test""/> class.
/// </summary>
/// <param name=""testAmount"">The test amount.</param>"));
        }
    }
}
