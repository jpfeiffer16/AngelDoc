using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentionGeneratorTests
{
    public class GenerateEnumDocs
    {
        private string _result;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = Substitute.For<IIdentifierHelper>();
            identifierHelper.ParseIdentifier("Test").Returns(new List<string> { "test" });
            var documentationGenerator = new DocumentionGenerator(identifierHelper);

            var enumDef = TestHelpers.GetSyntaxSymbol<EnumDeclarationSyntax>(
@"enum Test : AnotherEnum<string>
{
    ONE = 1
}");

            _result = documentationGenerator.GenerateEnumDocs(enumDef);
        }

        [Test]
        public void ResultIsCorrect()
        {
            Assert.That(_result, Is.EqualTo(
@"/// <summary>
/// Test.
/// </summary>
/// <seealso cref=""AnotherEnum{string}"" />"));
        }
    }
}
