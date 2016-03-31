namespace Quiztroller.Models
{
    public class Question
    {
        public Question(string number, int points, string category, string mainQuestion, string answer, string alternateQuestion)
        {
            this.Number = number;
            this.Points = points;
            this.Category = category;
            this.MainQuestion = mainQuestion;
            this.Answer = answer;
            this.AlternateQuestion = alternateQuestion;
        }

        public string Number { get; set; }

        public int Points { get; set; }

        public string Category { get; set; }

        public string MainQuestion { get; set; }

        public string AlternateQuestion { get; set; }

        public string Answer { get; set; }
    }
}
