namespace Quizbuilder.Models
{
    using System.Collections.Generic;

    public class SpreadsheetParseResult
    {
        public List<string> Questions { get; } = new List<string>();

        public List<string> Categories { get; } = new List<string>();

        public List<string> Answers { get; } = new List<string>();
    }
}