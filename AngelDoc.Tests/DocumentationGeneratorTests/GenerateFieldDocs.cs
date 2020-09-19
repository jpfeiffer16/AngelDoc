using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentionGeneratorTests
{
    public class GenerateFieldDocs
    {
        private object _result;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = Substitute.For<IIdentifierHelper>();
            identifierHelper.ParseIdentifier("EntityId").Returns(new List<string> { "entity", "id" });
            var documentationGenerator = new DocumentionGenerator(identifierHelper);

            var fieldDef = TestHelpers.GetSyntaxSymbol<FieldDeclarationSyntax>(
@"public class TestClass
{
    public int EntityId = 123;
}", 2);
            _result = documentationGenerator.GenerateFieldDocs(fieldDef);

        }

        [Test]
        public void ResultIsCorrect()
        {
            Assert.That(_result, Is.EqualTo(
@"/// <summary>
/// The entity id.
/// </summary>"));
        }
    }
}
