namespace Quiztroller.Models
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
        private int points11;
        private int points12;
        private int points13;
        private int points14;
        private int points15;
        private int points16;
        private int points17;
        private int points18;
        private int points19;
        private int points20;
        private int points21;
        private int points22;
        private int points23;
        private int points24;

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

        public int Points11
        {
            get
            {
                return this.points11;
            }
            set
            {
                this.Set(() => this.Points11, ref this.points11, value);
                this.CalculateScore();
            }
        }

        public int Points12
        {
            get
            {
                return this.points12;
            }
            set
            {
                this.Set(() => this.Points12, ref this.points12, value);
                this.CalculateScore();
            }
        }

        public int Points13
        {
            get
            {
                return this.points13;
            }
            set
            {
                this.Set(() => this.Points13, ref this.points13, value);
                this.CalculateScore();
            }
        }

        public int Points14
        {
            get
            {
                return this.points14;
            }
            set
            {
                this.Set(() => this.Points14, ref this.points14, value);
                this.CalculateScore();
            }
        }

        public int Points15
        {
            get
            {
                return this.points15;
            }
            set
            {
                this.Set(() => this.Points15, ref this.points15, value);
                this.CalculateScore();
            }
        }

        public int Points16
        {
            get
            {
                return this.points16;
            }
            set
            {
                this.Set(() => this.Points16, ref this.points16, value);
                this.CalculateScore();
            }
        }

        public int Points17
        {
            get
            {
                return this.points17;
            }
            set
            {
                this.Set(() => this.Points17, ref this.points17, value);
                this.CalculateScore();
            }
        }

        public int Points18
        {
            get
            {
                return this.points18;
            }
            set
            {
                this.Set(() => this.Points18, ref this.points18, value);
                this.CalculateScore();
            }
        }

        public int Points19
        {
            get
            {
                return this.points19;
            }
            set
            {
                this.Set(() => this.Points19, ref this.points19, value);
                this.CalculateScore();
            }
        }

        public int Points20
        {
            get
            {
                return this.points20;
            }
            set
            {
                this.Set(() => this.Points20, ref this.points20, value);
                this.CalculateScore();
            }
        }

        public int Points21
        {
            get
            {
                return this.points21;
            }
            set
            {
                this.Set(() => this.Points21, ref this.points21, value);
                this.CalculateScore();
            }
        }

        public int Points22
        {
            get
            {
                return this.points22;
            }
            set
            {
                this.Set(() => this.Points22, ref this.points22, value);
                this.CalculateScore();
            }
        }

        public int Points23
        {
            get
            {
                return this.points23;
            }
            set
            {
                this.Set(() => this.Points23, ref this.points23, value);
                this.CalculateScore();
            }
        }

        public int Points24
        {
            get
            {
                return this.points24;
            }
            set
            {
                this.Set(() => this.Points24, ref this.points24, value);
                this.CalculateScore();
            }
        }

        private void CalculateScore()
        {
            this.Score = this.Points1 + this.Points2 + this.Points3 + this.Points4 + this.Points5 + this.Points6 + this.Points7 + this.Points8 + this.Points9 + this.Points10
                         + this.Points11 + this.Points12 + this.Points13 + this.Points14 + this.Points15 + this.Points16 + this.Points17 + this.Points18 + this.Points19 + this.Points20
                         + this.Points21 + this.Points22 + this.Points23 + this.Points24;
        }
    }
}
