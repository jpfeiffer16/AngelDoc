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
                    {
                        // TODO: Check args[2] exists and can be parsed
                        GenDoc(int.Parse(args[2]), code);
                        break;
                    }
                default:
                    Environment.Exit(1);
                    break;
            }
        }

        private static void GenDoc(int lineNumber, string code)
        {
            var identifierHelper = new IdentifierHelper();
            var documentationGenerator = new DocumentionGenerator(identifierHelper);
            var xmlDocCreator = new XmlDocCreator(documentationGenerator); 

            var outString = xmlDocCreator.CreateDocLines(lineNumber, code);

            Console.Write(outString);
        }
    }
}
