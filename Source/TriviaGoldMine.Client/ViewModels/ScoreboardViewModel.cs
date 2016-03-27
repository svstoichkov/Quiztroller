namespace TriviaGoldMine.Client.ViewModels
{
    using System.Collections.ObjectModel;

    using GalaSoft.MvvmLight;

    using Models;

    public class ScoreboardViewModel : ViewModelBase
    {
        public ScoreboardViewModel()
        {
            for (int i = 0; i < 50; i++)
            {
                this.Teams.Add(new Team());
            }
        }

        public ObservableCollection<Team> Teams { get; set; } = new ObservableCollection<Team>();
    }
}
