using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentionGeneratorTests
{
    public class GenerateMethodDocs
    {
        private string _pluralResult;
        private string _singularResult;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = Substitute.For<IIdentifierHelper>();
            identifierHelper
                .ParseIdentifier("GetSomething")
                .Returns(
                    new List<string> { "get", "something" });
            identifierHelper
                .ParseIdentifier("Main")
                .Returns(
                    new List<string> { "main" });
            identifierHelper
                .ParseIdentifier("id")
                .Returns(new List<string> { "id" });
            identifierHelper
                .ParseIdentifier("name")
                .Returns(new List<string> { "name" });
            identifierHelper
                .ParseIdentifier("args")
                .Returns(new List<string> { "args" });
            identifierHelper
                .ParseIdentifier("TStuff")
                .Returns(new List<string> { "t", "stuff" });

            var documentationGenerator = new DocumentionGenerator(identifierHelper);

            var pluralMethodDef = TestHelpers.GetSyntaxSymbol<MethodDeclarationSyntax>(
@"public class TestClass
{
    public void GetSomething<TStuff>(int id, string name)
    {
    }
}", 2);

            var singularMethodDef = TestHelpers.GetSyntaxSymbol<MethodDeclarationSyntax>(
@"public class TestClass
{
    public void Main(string[] args)
    {
    }
}", 2);

            _pluralResult = documentationGenerator.GenerateMethodDocs(pluralMethodDef);
            _singularResult = documentationGenerator.GenerateMethodDocs(singularMethodDef);
        }

        [Test]
        public void PluralResultIsCorrect()
        {
            Assert.That(_pluralResult, Is.EqualTo(
@"/// <summary>
/// Gets something.
/// </summary>
/// <param name=""id"">The id.</param>
/// <param name=""name"">The name.</param>
/// <typeparam name=""TStuff"">The stuff type.</typeparam>"));
        }

        [Test]
        public void SingularResultIsCorrect()
        {
            Assert.That(_singularResult, Is.EqualTo(
@"/// <summary>
/// Main.
/// </summary>
/// <param name=""args"">The args.</param>"));
        }
    }
}
