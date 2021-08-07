using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pluralize.NET;

namespace AngelDoc
{
    /// <summary>
    /// Documention generator.
    /// </summary>
    /// <seealso cref="IDocumentationGenerator" />
    public class DocumentionGenerator : IDocumentationGenerator
    {
        private const string SummaryTemplate =
@"/// <summary>
/// {0}.
/// </summary>";
        private const string ParamTemplate = @"
/// <param name=""{0}"">The {1}.</param>";
        private const string SeeTemplate = "<see cref=\"{0}\"/>";
        private const string ValueTemplate = @"
/// <value>
/// {0}.
/// </value>";
        private const string SeeAlsoTemplate = @"
/// <seealso cref=""{0}"" />";
        private const string TypeParamTemplate = @"
/// <typeparam name=""{0}"">{1}</typeparam>";
        private const string ExceptionTemplate = @"
/// <exception cref=""{0}"">{0} error.</exception>";

        private readonly IIdentifierHelper _identifierHelper;
        private readonly Pluralizer _pluralizer = new Pluralizer();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentionGenerator"/> class.
        /// </summary>
        public DocumentionGenerator(IIdentifierHelper identifierHelper)
        {
            _identifierHelper = identifierHelper;
        }

        /// <inheritdoc/>
        public string GenerateClassDocs(ClassDeclarationSyntax classDeclaration)
        {
            var identifierList = _identifierHelper.ParseIdentifier(classDeclaration.Identifier.Value.ToString());
            identifierList[0] = identifierList[0][0].ToString().ToUpper() + identifierList[0].Substring(1);

            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat(SummaryTemplate, string.Join(" ", identifierList));
            if (classDeclaration.BaseList?.Types.Count > 0)
            {
                foreach (var baseType in classDeclaration.BaseList.Types)
                {
                    docBuilder.AppendFormat(SeeAlsoTemplate, FormatSeeAlsoTypeName(baseType.ToString()));
                }
            }

            GenerateTypeParams(classDeclaration.TypeParameterList, docBuilder);

            return docBuilder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateInterfaceDocs(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            var identifierList = _identifierHelper.ParseIdentifier(interfaceDeclaration.Identifier.Value.ToString());
            if (identifierList.FirstOrDefault() == "i") identifierList = identifierList.Skip(1).ToList();
            identifierList[0] = identifierList[0][0].ToString().ToUpper() + identifierList[0].Substring(1);

            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat(SummaryTemplate, string.Join(" ", identifierList));
            if (interfaceDeclaration.BaseList?.Types.Count > 0)
            {
                foreach (var baseType in interfaceDeclaration.BaseList.Types)
                {
                    docBuilder.AppendFormat(SeeAlsoTemplate, FormatSeeAlsoTypeName(baseType.ToString()));
                }
            }

            GenerateTypeParams(interfaceDeclaration.TypeParameterList, docBuilder);

            return docBuilder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateMethodDocs(MethodDeclarationSyntax methodDeclaration)
        {
            var methodName = methodDeclaration.Identifier.Value.ToString();
            var docBuilder = new StringBuilder();
            // Special cases
            if (methodName == "Dispose")
            {
                docBuilder.AppendFormat(
                    SummaryTemplate,
                    string.Format("Disposes the {0} instance and its resources",
                        string.Format(SeeTemplate, (methodDeclaration.Parent as ClassDeclarationSyntax)?.Identifier.Text)));
            }
            else
            {
                var identifierList = _identifierHelper.ParseIdentifier(methodName);
                if (identifierList.Count > 1)
                {
                    identifierList[0] = _pluralizer.Pluralize(identifierList[0]);
                }
                identifierList[0] = identifierList[0][0].ToString().ToUpper()
                    + identifierList[0].Substring(1);

                docBuilder.AppendFormat(SummaryTemplate, string.Join(" ", identifierList));
            }

            foreach (var param in methodDeclaration.ParameterList.Parameters)
            {
                var parameterIdentifierList = _identifierHelper.ParseIdentifier(param.Identifier.Value.ToString());
                docBuilder.AppendFormat(ParamTemplate, param.Identifier.Value, string.Join(" ", parameterIdentifierList));
            }

            GenerateTypeParams(methodDeclaration.TypeParameterList, docBuilder);
            GenerateExceptions(methodDeclaration, docBuilder);

            return docBuilder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateConstructorDocs(ConstructorDeclarationSyntax ctorDeclaration)
        {
            var className = ctorDeclaration.Identifier.Value.ToString();

            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat(
                SummaryTemplate, string.Format(
                    "Initializes a new instance of the {0} {1}",
                    string.Format(SeeTemplate, className), ctorDeclaration.Parent is ClassDeclarationSyntax ? "class" : "struct"));
            foreach (var param in ctorDeclaration.ParameterList.Parameters)
            {
                var parameterIdentifierList = _identifierHelper.ParseIdentifier(param.Identifier.Value.ToString());
                docBuilder.AppendFormat(ParamTemplate, param.Identifier.Value, string.Join(" ", parameterIdentifierList));

            }

            return docBuilder.ToString();
        }

        /// <inheritdoc/>
        public string GeneratePropertyDocs(PropertyDeclarationSyntax propertyDeclaration)
        {
            // TODO: Need to fill this out more.
            var docBuilder = new StringBuilder();
            string accessorText = string.Empty;
            if (propertyDeclaration.AccessorList.Accessors.Count == 2)
                accessorText = "Gets or sets the ";
            else if (propertyDeclaration.AccessorList.Accessors.FirstOrDefault()?.Keyword.Text == "get")
                accessorText = "Gets the ";
            else if (propertyDeclaration.AccessorList.Accessors.FirstOrDefault()?.Keyword.Text == "set")
                accessorText = "Sets the ";
            var identifierList = _identifierHelper.ParseIdentifier(propertyDeclaration.Identifier.Text);
            var summaryText = accessorText + string.Join(" ", identifierList);
            var valueText = "The " + string.Join(" ", identifierList);

            docBuilder.AppendFormat(SummaryTemplate, summaryText);
            docBuilder.AppendFormat(ValueTemplate, valueText);

            return docBuilder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateFieldDocs(FieldDeclarationSyntax fieldDeclaration)
        {
            var identifierList = _identifierHelper.ParseIdentifier(fieldDeclaration.Declaration.Variables.FirstOrDefault().Identifier.Value.ToString());
            identifierList.Insert(0, "the");
            identifierList[0] = identifierList[0][0].ToString().ToUpper() + identifierList[0].Substring(1);

            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat(SummaryTemplate, string.Join(" ", identifierList));

            return docBuilder.ToString();
        }

        /// <inheritdoc />
        public string GenerateEnumDocs(EnumDeclarationSyntax enumDeclaration)
        {
            var identifierList = _identifierHelper.ParseIdentifier(enumDeclaration.Identifier.Value.ToString());
            identifierList[0] = identifierList[0][0].ToString().ToUpper() + identifierList[0].Substring(1);

            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat(SummaryTemplate, string.Join(" ", identifierList));
            if (enumDeclaration.BaseList?.Types.Count > 0)
            {
                foreach (var baseType in enumDeclaration.BaseList.Types)
                {
                    docBuilder.AppendFormat(SeeAlsoTemplate, FormatSeeAlsoTypeName(baseType.ToString()));
                }
            }

            return docBuilder.ToString();
        }

        /// <inheritdoc />
        public string GenerateStructDocs(StructDeclarationSyntax structDeclaration)
        {
            var identifierList = _identifierHelper.ParseIdentifier(structDeclaration.Identifier.Value.ToString());
            identifierList[0] = identifierList[0][0].ToString().ToUpper() + identifierList[0].Substring(1);

            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat(SummaryTemplate, string.Join(" ", identifierList));
            if (structDeclaration.BaseList?.Types.Count > 0)
            {
                foreach (var baseType in structDeclaration.BaseList.Types)
                {
                    docBuilder.AppendFormat(SeeAlsoTemplate, FormatSeeAlsoTypeName(baseType.ToString()));
                }
            }

            GenerateTypeParams(structDeclaration.TypeParameterList, docBuilder);

            return docBuilder.ToString();
        }

        #region Helper Methods

        private void GenerateTypeParams(TypeParameterListSyntax typeParamList, StringBuilder docBuilder)
        {
            if (typeParamList?.Parameters.Count > 0)
            {
                foreach (var typeParam in typeParamList.Parameters)
                {
                    var description = string.Empty;
                    var typeParamIdentifiers = _identifierHelper.ParseIdentifier(typeParam.Identifier.ToString());
                    if (typeParamIdentifiers.Count > 1)
                    {
                        if (typeParamIdentifiers.FirstOrDefault() == "t")
                        {
                            typeParamIdentifiers = typeParamIdentifiers.Skip(1).ToList();
                        }
                        description = $"The {string.Join(" ", typeParamIdentifiers)} type.";
                    }
                    docBuilder.AppendFormat(TypeParamTemplate, typeParam.Identifier.ToString(), description);
                }
            }
        }

        private void GenerateExceptions(MethodDeclarationSyntax methodDeclaration, StringBuilder docBuilder)
        {
            var throwStatements = GetAllThrowStatements(methodDeclaration.Body);
            foreach (var throwStatment in throwStatements)
            {
                var errorNode = throwStatment.ChildNodes().FirstOrDefault();
                if (errorNode.IsKind(SyntaxKind.ObjectCreationExpression))
                {
                    var objectInitializer = errorNode as ObjectCreationExpressionSyntax;
                    docBuilder.AppendFormat(ExceptionTemplate, objectInitializer.Type.ToString());
                }
            }
        }

        private List<ThrowStatementSyntax> GetAllThrowStatements(SyntaxNode node, List<ThrowStatementSyntax> throwStatements = null)
        {
            throwStatements ??= new List<ThrowStatementSyntax>();

            if (node is null) return throwStatements;

            if (node.IsKind(SyntaxKind.ThrowStatement))
            {
                throwStatements.Add(node as ThrowStatementSyntax);
            }
            else
            {
                var childNodes = node.ChildNodes();
                if (childNodes.Any())
                {
                    foreach (var n in childNodes)
                    {
                        GetAllThrowStatements(n, throwStatements);
                    }
                }
            }

            return throwStatements;
        }
        
        private string FormatSeeAlsoTypeName(string typeName)
        {
            return typeName.Replace("<", "{").Replace(">", "}");
        }

        #endregion
    }
}
