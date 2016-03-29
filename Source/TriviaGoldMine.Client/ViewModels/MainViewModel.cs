namespace Quiztroller.ViewModels
{
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using Properties;

    using Views;

    public class MainViewModel : ViewModelBase
    {
        private string switcherContent = "Scoreboard";
        private readonly object questions = new Questions();
        private readonly object scoreboard = new Scoreboard();
        private readonly object howToUse = new HowToUse();
        private object currentContent;

        public MainViewModel()
        {
            this.currentContent = this.questions;
            this.switcherContent = "Scoreboard";
            if (Settings.Default.HowToUse)
            {
                this.currentContent = this.howToUse;
                this.switcherContent = "Questions";
            }
            this.Switch = new RelayCommand(this.HandleSwitch);
            this.HowToUse = new RelayCommand(this.HandleHowToUse, this.CanHowToUse);
        }

        private bool CanHowToUse()
        {
            return this.CurrentContent != this.howToUse;
        }

        private void HandleHowToUse()
        {
            this.CurrentContent = this.howToUse;
            this.SwitcherContent = "Questions";
        }

        public ICommand Switch { get; set; }

        public ICommand HowToUse { get; set; }

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