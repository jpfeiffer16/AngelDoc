using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentationGeneratorTests
{
    public class GenerateConstructorDocs
    {
        private string _classResult;
        private string _structResult;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = Substitute.For<IIdentifierHelper>();
            identifierHelper.ParseIdentifier("Test").Returns(new List<string> { "test" });
            identifierHelper.ParseIdentifier("testAmount").Returns(new List<string> { "test", "amount" });
            var documentationGenerator = new DocumentionGenerator(identifierHelper);

            var ctorDefClass = TestHelpers.GetSyntaxSymbol<ConstructorDeclarationSyntax>(
@"class Test
{
    public Test(double testAmount)
    {
    }
}", 2);

            var ctorDefStruct = TestHelpers.GetSyntaxSymbol<ConstructorDeclarationSyntax>(
@"struct Test
{
    public Test(double testAmount)
    {
    }
}", 2);

            _classResult = documentationGenerator.GenerateConstructorDocs(ctorDefClass);
            _structResult = documentationGenerator.GenerateConstructorDocs(ctorDefStruct);
        }

        [Test]
        public void ClassResultIsCorrect()
        {
            Assert.That(_classResult, Is.EqualTo(
@"/// <summary>
/// Initializes a new instance of the <see cref=""Test""/> class.
/// </summary>
/// <param name=""testAmount"">The test amount.</param>"));
        }

        [Test]
        public void StructResultIsCorrect()
        {
            Assert.That(_structResult, Is.EqualTo(
@"/// <summary>
/// Initializes a new instance of the <see cref=""Test""/> struct.
/// </summary>
/// <param name=""testAmount"">The test amount.</param>"));
        }
    }
}
