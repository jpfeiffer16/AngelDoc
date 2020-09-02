using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests.DocumentionGeneratorTests
{
    public class GenerateClassDocs
    {
        private DocumentionGenerator _documentationGenerator;

        [SetUp]
        public void Setup()
        {
            _documentationGenerator = new DocumentionGenerator(Substitute.For<IIdentifierHelper>());

            // _documentationGenerator.GenerateClassDocs(ClassDeclarationSyntax);
        }

        [Test]
        public void Test()
        {
            Assert.Fail();
        }
        // public class TestClass
        // {
        // }
    }
}
