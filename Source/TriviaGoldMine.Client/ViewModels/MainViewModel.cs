namespace TriviaGoldMine.Client.ViewModels
{
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Views;

    public class MainViewModel : ViewModelBase
    {
        private string switcherContent = "Scoreboard";
        private readonly object questions = new Questions();
        private readonly object scoreboard = new Scoreboard();
        private object currentContent;

        public MainViewModel()
        {
            this.currentContent = questions;
            this.Switch = new RelayCommand(this.HandleSwitch);
        }

        public ICommand Switch { get; set; }

        public string SwitcherContent
        {
            get
            {
                return this.switcherContent;
            }
            set
            {
                this.Set(() => this.SwitcherContent, ref this.switcherContent, value);
            }
        }

        public object CurrentContent
        {
            get
            {
                return this.currentContent;
            }
            set
            {
                this.Set(() => this.CurrentContent, ref this.currentContent, value);
            }
        }

        private void HandleSwitch()
        {
            if (this.CurrentContent == this.questions)
            {
                this.CurrentContent = this.scoreboard;
                this.SwitcherContent = "Questions";
            }
            else
            {
                this.CurrentContent = this.questions;
                this.SwitcherContent = "Scoreboard";
            }
        }
    }
}