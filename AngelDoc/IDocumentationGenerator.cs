using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AngelDoc
{
    public interface IDocumentationGenerator
    {
        /// <summary>
        /// Generates method docs.
        /// </summary>
        /// <param name="methodDeclaration">The method declaration.</param>
        public string GenerateMethodDocs(MethodDeclarationSyntax methodDeclaration);

        /// <summary>
        /// Generates class docs.
        /// </summary>
        /// <param name="classDeclaration">The class declaration.</param>
        public string GenerateClassDocs(ClassDeclarationSyntax classDeclaration);

        /// <summary>
        /// Generates constructor docs.
        /// </summary>
        /// <param name="ctorDeclaration">The ctor declaration.</param>
        public string GenerateConstructorDocs(ConstructorDeclarationSyntax ctorDeclaration);

        /// <summary>
        /// Generates interface docs.
        /// </summary>
        /// <param name="interfaceDeclaration">The interface declaration.</param>
        string GenerateInterfaceDocs(InterfaceDeclarationSyntax interfaceDeclaration);

        /// <summary>
        /// Generates property docs.
        /// </summary>
        /// <param name="propertyDeclaration">The property declaration.</param>
        string GeneratePropertyDocs(PropertyDeclarationSyntax propertyDeclaration);

        /// <summary>
        /// Generates field docs.
        /// </summary>
        /// <param name="propertyDeclaration">The property declaration.</param>
        string GenerateFieldDocs(FieldDeclarationSyntax propertyDeclaration);

        /// <summary>
        /// Generates enum docs.
        /// </summary>
        /// <param name="enumDeclaration">The enum declaration.</param>
        string GenerateEnumDocs(EnumDeclarationSyntax enumDeclaration);

        /// <summary>
        /// Generates struct docs.
        /// </summary>
        /// <param name="structDeclaration">The struct declaration.</param>
        string GenerateStructDocs(StructDeclarationSyntax structDeclaration);
    }
}
