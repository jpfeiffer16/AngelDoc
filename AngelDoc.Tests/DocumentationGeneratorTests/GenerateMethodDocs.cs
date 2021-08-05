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
        private string _disposeResult;
        private string _exceptionThrowingResult;
        private string _interfaceDefResult;

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
            identifierHelper
                .ParseIdentifier("parameter")
                .Returns(new List<string> { "parameter" });
            identifierHelper
                .ParseIdentifier("ThrowsException")
                .Returns(new List<string> { "throws", "exception" });
            identifierHelper
                .ParseIdentifier("Test")
                .Returns(new List<string> { "test" });

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


            var disposeMethodDef = TestHelpers.GetSyntaxSymbol<MethodDeclarationSyntax>(
@"public class TestClass : IDisposable
{
    public void Dispose(string parameter)
    {
    }
}", 2);
            var exceptionThrowingMethodDef = TestHelpers.GetSyntaxSymbol<MethodDeclarationSyntax>(
@"public class TestClass
{
    public void ThrowsException()
    {
        if (1 == 1)
        {
            throw new Exception(""test"");
        }
        else
        {
            throw new HttpException(""http test"");
        }
    }
}", 2);
            var interfaceMethodDef = TestHelpers.GetSyntaxSymbol<MethodDeclarationSyntax>(
@"public class ITestInterface
{
    public void Test();
}", 2);
            _pluralResult = documentationGenerator.GenerateMethodDocs(pluralMethodDef);
            _singularResult = documentationGenerator.GenerateMethodDocs(singularMethodDef);
            _disposeResult = documentationGenerator.GenerateMethodDocs(disposeMethodDef);
            _exceptionThrowingResult = documentationGenerator.GenerateMethodDocs(exceptionThrowingMethodDef);
            _interfaceDefResult = documentationGenerator.GenerateMethodDocs(interfaceMethodDef);
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

        [Test]
        public void DisposeResultIsCorrect()
        {
            Assert.That(_disposeResult, Is.EqualTo(
@"/// <summary>
/// Disposes the <see cref=""TestClass""/> instance and its resources.
/// </summary>
/// <param name=""parameter"">The parameter.</param>"));
        }

        [Test]
        public void ExceptionThrowingResultIsCorrect()
        {
            Assert.That(_exceptionThrowingResult, Is.EqualTo(
@"/// <summary>
/// Throws exception.
/// </summary>
/// <exception cref=""Exception"">Exception error.</exception>
/// <exception cref=""HttpException"">HttpException error.</exception>"));
        }

        [Test]
        public void InterfaceDefResultIsCorrect()
        {
            Assert.That(_interfaceDefResult, Is.EqualTo(
@"/// <summary>
/// Test.
/// </summary>"));
        }
    }
}
