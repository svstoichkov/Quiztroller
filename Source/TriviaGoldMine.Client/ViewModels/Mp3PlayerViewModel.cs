namespace TriviaGoldMine.Client.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Media;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class Mp3PlayerViewModel : ViewModelBase
    {
        private int currentSongIndex;
        private string currentSong = "Music Player";
        private readonly MediaPlayer mediaPlayer = new MediaPlayer();
        private bool isPlaying;
        private bool popUpEnabled;

        public Mp3PlayerViewModel()
        {
            this.Play = new RelayCommand(this.HandlePlay, this.CanPlay);
            this.Pause = new RelayCommand(this.HandlePause, this.CanPause);
            this.Previous = new RelayCommand(this.HandlePrevious, this.HasMp3);
            this.Next = new RelayCommand(this.HandleNext, this.HasMp3);
        }

        public ICommand Play { get; set; }

        public ICommand Pause { get; set; }

        public ICommand Previous { get; set; }

        public ICommand Next { get; set; }

        public ObservableCollection<string> Songs { get; set; } = new ObservableCollection<string>();

        public bool PopUpEnabled
        {
            get
            {
                return this.popUpEnabled;
            }
            set
            {
                this.Set(() => this.PopUpEnabled, ref this.popUpEnabled, value);
            }
        }

        public string CurrentSong
        {
            get
            {
                return this.currentSong;
            }
            set
            {
                this.Set(() => this.CurrentSong, ref this.currentSong, value);

                this.mediaPlayer.Open(new Uri(value));
                if (this.isPlaying)
                {
                    this.mediaPlayer.Play();
                }
            }
        }

        public void AddSongs(string[] mp3)
        {
            this.Songs.Clear();
            foreach (var filename in mp3)
            {
                this.Songs.Add(filename);
            }
            this.PopUpEnabled = true;
            this.CurrentSong = this.Songs.First();
        }

        private bool HasMp3()
        {
            return this.Songs.Any();
        }

        private void HandleNext()
        {
            if (this.currentSongIndex == this.Songs.Count - 1)
            {
                this.currentSongIndex = 0;
            }
            else
            {
                this.currentSongIndex++;
            }

            this.CurrentSong = this.Songs[this.currentSongIndex];

        }

        private void HandlePrevious()
        {
            if (this.currentSongIndex == 0)
            {
                this.currentSongIndex = this.Songs.Count - 1;
            }
            else
            {
                this.currentSongIndex--;
            }

            this.CurrentSong = this.Songs[this.currentSongIndex];
        }

        private bool CanPlay()
        {
            return !this.isPlaying && this.Songs.Any();
        }

        private bool CanPause()
        {
            return this.isPlaying;
        }

        private void HandlePlay()
        {
            this.mediaPlayer.Play();
            this.isPlaying = true;
        }

        private void HandlePause()
        {
            this.mediaPlayer.Pause();
            this.isPlaying = false;
        }
    }
}
