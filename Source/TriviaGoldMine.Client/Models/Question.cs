namespace TriviaGoldMine.Client.Models
{
    public class Question
    {
        public Question(int number, int points, string mainQuestion, string answer, string alternateQuestion)
        {
            this.Number = number;
            this.Points = points;
            this.MainQuestion = mainQuestion;
            this.Answer = answer;
            this.AlternateQuestion = alternateQuestion;
        }

        public int Number { get; set; }

        public int Points { get; set; }

        public string MainQuestion { get; set; }

        public string AlternateQuestion { get; set; }

        public string Answer { get; set; }
    }
}
