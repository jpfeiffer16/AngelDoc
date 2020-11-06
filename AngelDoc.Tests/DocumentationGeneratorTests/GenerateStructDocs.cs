using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentionGeneratorTests
{
    public class GenerateStructDocs
    {
        private string _result;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = Substitute.For<IIdentifierHelper>();
            identifierHelper.ParseIdentifier("Test").Returns(new List<string> { "test" });
            var documentationGenerator = new DocumentionGenerator(identifierHelper);

            var ctorDef = TestHelpers.GetSyntaxSymbol<StructDeclarationSyntax>("struct Test : IStructBase { }");

            _result = documentationGenerator.GenerateStructDocs(ctorDef);
        }

        [Test]
        public void ResultIsCorrect()
        {
            Assert.That(_result, Is.EqualTo(
@"/// <summary>
/// Test.
/// </summary>
/// <seealso cref=""IStructBase"" />"));
        }
    }
}
