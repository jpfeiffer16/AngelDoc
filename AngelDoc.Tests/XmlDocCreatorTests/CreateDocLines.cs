using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using NUnit.Framework;

namespace AngelDoc.Tests
{
    public class CreateDocLines
    {
        private IDocumentationGenerator _documentationGenerator;
        private XmlDocCreator _xmlDocCreator;
        private int _generateClassDocs;
        private int _generateInterfaceDocs;
        private int _generateMethodDocs;
        private int _generateConstructorDocs;
        private int _generatePropertyDocs;
        private int _generateFieldDocs;
        private int _generateEnumDocs;
        private int _generateStructDocs;

        [SetUp]
        public void Setup()
        {
            _generateClassDocs = 0;
            _generateInterfaceDocs = 0;
            _generateMethodDocs = 0;
            _generateConstructorDocs = 0;
            _generatePropertyDocs = 0;
            _generateFieldDocs = 0;
            _generateEnumDocs = 0;
            _generateStructDocs = 0;

            _documentationGenerator = Substitute.For<IDocumentationGenerator>();

            _documentationGenerator.When(x =>
                x.GenerateClassDocs(Arg.Any<ClassDeclarationSyntax>())).Do(_ => _generateClassDocs++);
            _documentationGenerator.When(x =>
                x.GenerateInterfaceDocs(Arg.Any<InterfaceDeclarationSyntax>())).Do(_ => _generateInterfaceDocs++);
            _documentationGenerator.When(x =>
                x.GenerateMethodDocs(Arg.Any<MethodDeclarationSyntax>())).Do(_ => _generateMethodDocs++);
            _documentationGenerator.When(x =>
                x.GenerateConstructorDocs(Arg.Any<ConstructorDeclarationSyntax>())).Do(_ => _generateConstructorDocs++);
            _documentationGenerator.When(x =>
                x.GeneratePropertyDocs(Arg.Any<PropertyDeclarationSyntax>())).Do(_ => _generatePropertyDocs++);
            _documentationGenerator.When(x =>
                x.GenerateFieldDocs(Arg.Any<FieldDeclarationSyntax>())).Do(_ => _generateFieldDocs++);
            _documentationGenerator.When(x =>
                x.GenerateEnumDocs(Arg.Any<EnumDeclarationSyntax>())).Do(_ => _generateEnumDocs++);
            _documentationGenerator.When(x =>
                x.GenerateStructDocs(Arg.Any<StructDeclarationSyntax>())).Do(_ => _generateStructDocs++);

            _xmlDocCreator = new XmlDocCreator(_documentationGenerator);

            _xmlDocCreator.CreateDocLines(1, "class Test {}");
            _xmlDocCreator.CreateDocLines(1, "interface ITest {}");
            _xmlDocCreator.CreateDocLines(3,
@"class Test
{
    public int Method() { }
}");
            _xmlDocCreator.CreateDocLines(3, 
@"class Test
{
    public Test() { }
}");
            _xmlDocCreator.CreateDocLines(1, "public int Test {get;set;}");
            _xmlDocCreator.CreateDocLines(1, "public int Test;");
            _xmlDocCreator.CreateDocLines(1, "public enum Test");
            _xmlDocCreator.CreateDocLines(1, "public struct Test {}");
        }

        [Test]
        public void GenerateClassDocsIsCalled()
        {
            Assert.That(_generateClassDocs, Is.EqualTo(1));
        }

        [Test]
        public void GenerateInterfaceDocsIsCalled()
        {
            Assert.That(_generateInterfaceDocs, Is.EqualTo(1));
        }

        [Test]
        public void GenerateMethodDocsIsCalled()
        {
            Assert.That(_generateMethodDocs, Is.EqualTo(1));
        }

        [Test]
        public void GenerateConstructorDocsIsCalled()
        {
            Assert.That(_generateConstructorDocs, Is.EqualTo(1));
        }

        [Test]
        public void GeneratePropertyDocsIsCalled()
        {
            Assert.That(_generatePropertyDocs, Is.EqualTo(1));
        }

        [Test]
        public void GenerateFieldDocsIsCalled()
        {
            Assert.That(_generateFieldDocs, Is.EqualTo(1));
        }

        [Test]
        public void GenerateEnumDocsIsCalled()
        {
            Assert.That(_generateEnumDocs, Is.EqualTo(1));
        }

        [Test]
        public void GenerateStructDocsIsCalled()
        {
            Assert.That(_generateStructDocs, Is.EqualTo(1));
        }

    }
}
