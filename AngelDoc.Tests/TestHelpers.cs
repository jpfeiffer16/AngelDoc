
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;

namespace AngelDoc.Tests
{
    public static class TestHelpers
    {
        /// <summary>
        /// Gets syntax symbol.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="lineNumber">The line number. This is a 0 based index.</param>
        public static T GetSyntaxSymbol<T>(string code, int lineNumber = 0) where T : class
        {
            var tree = CSharpSyntaxTree.ParseText(code);
            var root = tree.GetCompilationUnitRoot();
            var lineSpan = tree.GetText().Lines[lineNumber].Span;
            var result = root.DescendantNodes()
                .Where(n => n.FullSpan.Contains(lineSpan)).LastOrDefault();
            return result as T;
        }
    }
}
