using System.Collections.Generic;

namespace AngelDoc
{
    /// <summary>
    /// Identifier helper.
    /// </summary>
    public interface IIdentifierHelper
    {
        /// <summary>
        /// Parses identifier.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public List<string> ParseIdentifier(string identifier);
    }
}
