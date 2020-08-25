using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Pluralize.NET;

namespace CodeAngel
{
    public class DocumentionGenerator : IDocumentationGenerator
    {
        private IIdentifierHelper _identifierHelper;

        private const string SummaryTemplate = @"
/// <summary>
/// {0}.
/// </summary>";
        private const string ParamTemplate = @"
/// <param name=""{0}"">The {1}.</param>";
        private const string SeeTemplate = "<see cref=\"{0}\"/>"; 
        private const string InheritDocTemplate = "/// <inheritdoc/>";
        private const string ValueTemplate = @"
/// <value>
/// {0}
/// </value>";
        private const string SeeAlsoTemplate = @"
/// <seealso cref=""{0}"" />";

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
            var pluralizer = new Pluralizer();
            identifierList[0] = identifierList[0][0].ToString().ToUpper() + identifierList[0].Substring(1);

            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat(SummaryTemplate, string.Join(" ", identifierList));
            if (classDeclaration.BaseList.Types.Count > 0)
            {
                foreach (var baseType in classDeclaration.BaseList.Types)
                {
                    docBuilder.AppendFormat(SeeAlsoTemplate, baseType.ToString());
                }
            }

            return docBuilder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateInterfaceDocs(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            var identifierList = _identifierHelper.ParseIdentifier(interfaceDeclaration.Identifier.Value.ToString());
            if (identifierList.FirstOrDefault() == "i") identifierList = identifierList.Skip(1).ToList();
            var pluralizer = new Pluralizer();
            identifierList[0] = identifierList[0][0].ToString().ToUpper() + identifierList[0].Substring(1);

            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat(SummaryTemplate, string.Join(" ", identifierList));

            return docBuilder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateMethodDocs(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration.Parent is ClassDeclarationSyntax cd
                    && cd.BaseList?.Types.Count > 0)
            {
                return InheritDocTemplate;
            }
            var identifierList = _identifierHelper.ParseIdentifier(
                methodDeclaration.Identifier.Value.ToString());
            var pluralizer = new Pluralizer();
            identifierList[0] = pluralizer.Pluralize(identifierList[0]);
            identifierList[0] = identifierList[0][0].ToString().ToUpper()
                + identifierList[0].Substring(1);

            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat(SummaryTemplate, string.Join(" ", identifierList));

            foreach (var param in methodDeclaration.ParameterList.Parameters)
            {
                var parameterIdentifierList = _identifierHelper.ParseIdentifier(param.Identifier.Value.ToString());
                docBuilder.AppendFormat(ParamTemplate, param.Identifier.Value, string.Join(" ", parameterIdentifierList));
            }

            return docBuilder.ToString();
        }

        /// <inheritdoc/>
        public string GenerateConstructorDocs(ConstructorDeclarationSyntax ctorDeclaration)
        {
            var className = ctorDeclaration.Identifier.Value.ToString();

            var docBuilder = new StringBuilder();
            docBuilder.AppendFormat(
                SummaryTemplate, string.Format(
                    "Initializes a new instance of the {0} class",
                    string.Format(SeeTemplate, className)));
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
            docBuilder.AppendFormat(SummaryTemplate, string.Empty);
            docBuilder.AppendFormat(ValueTemplate, string.Empty);

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
    }
}
