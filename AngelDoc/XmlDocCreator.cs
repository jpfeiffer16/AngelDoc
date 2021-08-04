using System;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AngelDoc
{
    public class XmlDocCreator : IXmlDocCreator
    {
        private IDocumentationGenerator _documentationGenerator;

        public XmlDocCreator(IDocumentationGenerator documentationGenerator)
        {
            _documentationGenerator = documentationGenerator
                ?? throw new ArgumentNullException(nameof(documentationGenerator));
        }

        public string CreateDocLines(int lineNumber, string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetCompilationUnitRoot();
            var lineSpan = tree.GetText().Lines[lineNumber - 1].Span;
            var def = root.DescendantNodes()
                .Where(n =>
                    n.FullSpan.Contains(lineSpan)).LastOrDefault();

            var outString = string.Empty;

            switch (def)
            {
                case MethodDeclarationSyntax methodDef:
                    outString = _documentationGenerator.GenerateMethodDocs(methodDef);
                    break;
                case ClassDeclarationSyntax classDef:
                    outString = _documentationGenerator.GenerateClassDocs(classDef);
                    break;
                case ConstructorDeclarationSyntax ctorDef:
                    outString = _documentationGenerator.GenerateConstructorDocs(ctorDef);
                    break;
                case InterfaceDeclarationSyntax interfaceDef:
                    outString = _documentationGenerator.GenerateInterfaceDocs(interfaceDef);
                    break;
                case PropertyDeclarationSyntax propertyDef:
                    outString = _documentationGenerator.GeneratePropertyDocs(propertyDef);
                    break;
                case FieldDeclarationSyntax fieldDef:
                    outString = _documentationGenerator.GenerateFieldDocs(fieldDef);
                    break;
                case EnumDeclarationSyntax enumDef:
                    outString = _documentationGenerator.GenerateEnumDocs(enumDef);
                    break;
                case StructDeclarationSyntax structDef:
                    outString = _documentationGenerator.GenerateStructDocs(structDef);
                    break;
            }

            return outString;
        }
    }
}
