using System.Collections.Generic;

namespace AngelDoc
{
    public interface IIdentifierHelper
    {
        public List<string> ParseIdentifier(string identifier);
    }
}
