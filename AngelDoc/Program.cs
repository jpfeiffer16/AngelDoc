using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AngelDoc
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Invalid Args");
                Environment.Exit(1);
            }
            var inputFileName = args[0];
            var code = string.Empty;

            if (inputFileName == "-")
            {
                using var reader = new StreamReader(Console.OpenStandardInput());
                code = reader.ReadToEnd();
            }
            else
            {
                code = File.ReadAllText(inputFileName);
            }
            switch (args[1])
            {
                case "gendoccsharp":
                    // TODO: Check args[2] exists and can be parsed
                    GenDoc(int.Parse(args[2]), code);
                    break;
                default:
                    Environment.Exit(1);
                    break;
            }
        }

        private static void GenDoc(int lineNumber, string code)
        {
            IIdentifierHelper identifierHelper = new IdentifierHelper();
            IDocumentationGenerator documentationGenerator = new DocumentionGenerator(identifierHelper);

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
                    outString = documentationGenerator.GenerateMethodDocs(methodDef);
                    break;
                case ClassDeclarationSyntax classDef:
                    outString = documentationGenerator.GenerateClassDocs(classDef);
                    break;
                case ConstructorDeclarationSyntax ctorDef:
                    outString = documentationGenerator.GenerateConstructorDocs(ctorDef);
                    break;
                case InterfaceDeclarationSyntax interfaceDef:
                    outString = documentationGenerator.GenerateInterfaceDocs(interfaceDef);
                    break;
                case PropertyDeclarationSyntax propertyDef:
                    outString = documentationGenerator.GeneratePropertyDocs(propertyDef);
                    break;
                case FieldDeclarationSyntax fieldDef:
                    outString = documentationGenerator.GenerateFieldDocs(fieldDef);
                    break;
                case EnumDeclarationSyntax enumDef:
                    outString = documentationGenerator.GenerateEnumDocs(enumDef);
                    break;
                case StructDeclarationSyntax structDef:
                    outString = documentationGenerator.GenerateStructDocs(structDef);
                    break;
            }

            Console.Write(outString);
        }
    }
}
