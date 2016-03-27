namespace TriviaGoldMine.Client.Models
{
    using GalaSoft.MvvmLight;
    public class Team : ViewModelBase
    {
        private int score;
        private int points1;
        private int points2;
        private int points3;
        private int points4;
        private int points5;
        private int points6;
        private int points7;
        private int points8;
        private int points9;
        private int points10;

        public int Score
        {
            get
            {
                return this.score;
            }
            set
            {
                this.Set(() => this.Score, ref this.score, value);
            }
        }

        public string Name { get; set; }

        public int Points1
        {
            get
            {
                return this.points1;
            }
            set
            {
                this.Set(() => this.Points1, ref this.points1, value);
                this.CalculateScore();
            }
        }

        public int Points2
        {
            get
            {
                return this.points2;
            }
            set
            {

                this.Set(() => this.Points2, ref this.points2, value);
                this.CalculateScore();
            }
        }

        public int Points3
        {
            get
            {
                return this.points3;
            }
            set
            {
                this.Set(() => this.Points3, ref this.points3, value);
                this.CalculateScore();
            }
        }

        public int Points4
        {
            get
            {
                return this.points4;
            }
            set
            {
                this.Set(() => this.Points4, ref this.points4, value);
                this.CalculateScore();
            }
        }

        public int Points5
        {
            get
            {
                return this.points5;
            }
            set
            {
                this.Set(() => this.Points5, ref this.points5, value);
                this.CalculateScore();
            }
        }

        public int Points6
        {
            get
            {
                return this.points6;
            }
            set
            {
                this.Set(() => this.Points6, ref this.points6, value);
                this.CalculateScore();
            }
        }

        public int Points7
        {
            get
            {
                return this.points7;
            }
            set
            {
                this.Set(() => this.Points7, ref this.points7, value);
                this.CalculateScore();
            }
        }

        public int Points8
        {
            get
            {
                return this.points8;
            }
            set
            {
                this.Set(() => this.Points8, ref this.points8, value);
                this.CalculateScore();
            }
        }

        public int Points9
        {
            get
            {
                return this.points9;
            }
            set
            {
                this.Set(() => this.Points9, ref this.points9, value);
                this.CalculateScore();
            }
        }

        public int Points10
        {
            get
            {
                return this.points10;
            }
            set
            {
                this.Set(() => this.Points10, ref this.points10, value);
                this.CalculateScore();
            }
        }

        private void CalculateScore()
        {
            this.Score = this.Points1 + this.Points2 + this.Points3 + this.Points4 + this.Points5 + this.Points6 + this.Points7 + this.Points8 + this.Points9 + this.Points10;
        }
    }
}
