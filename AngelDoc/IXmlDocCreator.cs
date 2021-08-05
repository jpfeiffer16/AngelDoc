namespace AngelDoc
{
    /// <summary>
    /// Xml doc creator.
    /// </summary>
    public interface IXmlDocCreator
    {
        /// <summary>
        /// Creates doc lines.
        /// </summary>
        /// <param name="lineNumber">The line number.</param>
        /// <param name="code">The code.</param>
        public string CreateDocLines(int lineNumber, string code);
    }
}
