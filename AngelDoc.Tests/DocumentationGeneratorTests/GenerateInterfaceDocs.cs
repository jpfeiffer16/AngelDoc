using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentionGeneratorTests
{
    public class GenerateInterfaceDocs
    {
        private string _result;

        [SetUp]
        public void Setup()
        {
            var identifierHelper = Substitute.For<IIdentifierHelper>();
            identifierHelper.ParseIdentifier("ITest").Returns(new List<string> { "i", "test" });
            identifierHelper.ParseIdentifier("T").Returns(new List<string> { "t" });

            var documentationGenerator = new DocumentionGenerator(identifierHelper);

            var interfaceDef = TestHelpers.GetSyntaxSymbol<InterfaceDeclarationSyntax>(
@"interface ITest<T> : IBase<string>
{
}");

            _result = documentationGenerator.GenerateInterfaceDocs(interfaceDef);
        }

        [Test]
        public void ResultIsCorrect()
        {
            Assert.That(_result, Is.EqualTo(
@"/// <summary>
/// Test.
/// </summary>
/// <seealso cref=""IBase{string}"" />
/// <typeparam name=""T""></typeparam>"));
        }
    }
}
