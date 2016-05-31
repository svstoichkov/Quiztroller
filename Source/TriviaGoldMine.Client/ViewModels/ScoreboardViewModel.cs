namespace Quiztroller.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Threading;

    using GalaSoft.MvvmLight;

    using Models;

    public class ScoreboardViewModel : ViewModelBase
    {
        private readonly DispatcherTimer timer = new DispatcherTimer();

        public ScoreboardViewModel()
        {
            for (var i = 0; i < 25; i++)
            {
                var team = new Team();
                team.PropertyChanged += this.OnPropertyChanged;
                this.Teams.Add(team);
            }

            this.timer.Interval = TimeSpan.FromSeconds(10);
            this.timer.Tick += this.OnTick;
            //this.timer.Start();;
        }

        public ObservableCollection<Team> Teams { get; set; } = new ObservableCollection<Team>();

        public ObservableCollection<Team> Top3 { get; set; } = new ObservableCollection<Team>();

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var top3 = this.Teams.Where(x => x.Score > 0).OrderByDescending(x => x.Score).Take(3);
            this.Top3.Clear();
            foreach (var team in top3)
            {
                this.Top3.Add(team);
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            var top3 = this.Teams.OrderByDescending(x => x.Score).Take(3);
            this.Top3.Clear();
            foreach (var team in top3)
            {
                this.Top3.Add(team);
            }
        }
    }
}