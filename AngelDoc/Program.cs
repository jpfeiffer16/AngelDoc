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

            if (def is MethodDeclarationSyntax methodDef)
                outString = documentationGenerator.GenerateMethodDocs(methodDef);
            else if (def is ClassDeclarationSyntax classDef)
                outString = documentationGenerator.GenerateClassDocs(classDef);
            else if (def is ConstructorDeclarationSyntax ctorDef)
                outString = documentationGenerator.GenerateConstructorDocs(ctorDef);
            else if (def is InterfaceDeclarationSyntax interfaceDef)
                outString = documentationGenerator.GenerateInterfaceDocs(interfaceDef);
            else if (def is PropertyDeclarationSyntax propertyDef)
                outString = documentationGenerator.GeneratePropertyDocs(propertyDef);
            else if (def is FieldDeclarationSyntax fieldDef)
                outString = documentationGenerator.GenerateFieldDocs(fieldDef);

            Console.Write(outString);
        }
    }
}
