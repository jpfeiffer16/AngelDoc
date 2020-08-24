#define DEGUB

using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using Pluralize.NET;


namespace CodeAngel
{
    class Program
    {
        private const string LOG_PATH = "./log.txt";

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
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
            // var code = File.ReadAllText("./Test.cs");
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetCompilationUnitRoot();
            // tree.GetLocation
            // Microsoft.CodeAnalysis.CSharp.CSharpS
            var lineSpan = tree.GetText().Lines[lineNumber - 1].Span;
            var def = root.DescendantNodes()
                // .OfType<MethodDeclarationSyntax>()
                .Where(n => 
                    n.FullSpan.Contains(lineSpan)).LastOrDefault();

            if (def is MethodDeclarationSyntax methodDef)
            {
                var identifierList = ParseIdentifier(methodDef.Identifier.Value.ToString());
                var pluralizer = new Pluralizer();
                identifierList[0] = pluralizer.Pluralize(identifierList[0]);
                identifierList[0] = identifierList[0][0].ToString().ToUpper() + identifierList[0].Substring(1);

                var str = $@"
/// <summary>
/// {string.Join(" ", identifierList)}.
/// </summary>";
                
                foreach (var param in methodDef.ParameterList.Parameters)
                {
                    var parameterIdentifierList = ParseIdentifier(param.Identifier.Value.ToString());
                    str += $"\n/// <param name=\"{param.Identifier.Value}\">The {string.Join(" ", parameterIdentifierList)}.</param>";
                }

                Console.Write(str);
#if DEBUG
                File.AppendAllText("./log.txt", $"{str}\n");
#endif
            }
            else if (def is ClassDeclarationSyntax classDef)
            {
                var identifierList = ParseIdentifier(classDef.Identifier.Value.ToString());
                var pluralizer = new Pluralizer();
                // identifierList[0] = pluralizer.Pluralize(identifierList[0]);
                identifierList[0] = identifierList[0][0].ToString().ToUpper() + identifierList[0].Substring(1);

                var str = $@"
/// <summary>
/// {string.Join(" ", identifierList)}.
/// </summary>";
                
                Console.Write(str);
#if DEBUG
                File.AppendAllText("./log.txt", $"{str}\n");
#endif

            }

        }

        private static List<string> ParseIdentifier(string identifier)
        {
            var list = new List<string>();
            for (var i = 0; i < identifier.Length; i++)
            {
                var ch = identifier[i];
                if (char.IsUpper(ch) || i == 0)
                {
                    list.Add(string.Empty);
                }
                list[list.Count - 1] = list.LastOrDefault() + ch.ToString().ToLower();
            }

            return list;
        }
    }
}
