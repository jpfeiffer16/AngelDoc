using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentationGeneratorTests
{
    public class GeneratePropertyDocs
    {
        private const string Code = @"public class TestClass
{
    public string FirstName { get; set; }
    public string LastName { set; }
    public string FullName { get; }
}";
        private string _getSetResult;
        private string _setResult;
        private string _getResult;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = Substitute.For<IIdentifierHelper>();
            identifierHelper
                .ParseIdentifier("FirstName")
                .Returns(new List<string> { "first", "name" });
            identifierHelper
                .ParseIdentifier("LastName")
                .Returns(new List<string> { "last", "name" });
            identifierHelper
                .ParseIdentifier("FullName")
                .Returns(new List<string> { "full", "name" });

            var documentationGenerator = new DocumentionGenerator(identifierHelper);

            var getSetPropertyDef = TestHelpers.GetSyntaxSymbol<PropertyDeclarationSyntax>(Code, 2);
            var setProperytDef = TestHelpers.GetSyntaxSymbol<PropertyDeclarationSyntax>(Code, 3);
            var getProperytDef = TestHelpers.GetSyntaxSymbol<PropertyDeclarationSyntax>(Code, 4);

            _getSetResult = documentationGenerator.GeneratePropertyDocs(getSetPropertyDef);
            _setResult = documentationGenerator.GeneratePropertyDocs(setProperytDef);
            _getResult = documentationGenerator.GeneratePropertyDocs(getProperytDef);
        }

        [Test]
        public void GetSetResultIsCorrect()
        {
            Assert.That(_getSetResult, Is.EqualTo(
@"/// <summary>
/// Gets or sets the first name.
/// </summary>
/// <value>
/// The first name.
/// </value>"));
        }

        [Test]
        public void SetResultIsCorrect()
        {
            Assert.That(_setResult, Is.EqualTo(
@"/// <summary>
/// Sets the last name.
/// </summary>
/// <value>
/// The last name.
/// </value>"));
        }

        [Test]
        public void GetResultIsCorrect()
        {
            Assert.That(_getResult, Is.EqualTo(
@"/// <summary>
/// Gets the full name.
/// </summary>
/// <value>
/// The full name.
/// </value>"));
        }
    }
}
