using System.Collections.Generic;
using System.Linq;

namespace AngelDoc
{
    public class IdentifierHelper : IIdentifierHelper
    {
        public List<string> ParseIdentifier(string identifier)
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
