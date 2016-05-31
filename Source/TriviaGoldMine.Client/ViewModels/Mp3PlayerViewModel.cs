namespace Quiztroller.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Threading;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    public class Mp3PlayerViewModel : ViewModelBase
    {
        private readonly MediaPlayer mediaPlayer = new MediaPlayer();
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private string currentSong = "Music Player";

        private int currentSongIndex;
        private string elapsed = "0:00";
        private bool isPlaying;
        private string length = "0:00";
        private bool popUpEnabled;

        public Mp3PlayerViewModel()
        {
            this.Play = new RelayCommand(this.HandlePlay, this.CanPlay);
            this.Pause = new RelayCommand(this.HandlePause, this.CanPause);
            this.Previous = new RelayCommand(this.HandlePrevious, this.HasMp3);
            this.Next = new RelayCommand(this.HandleNext, this.HasMp3);

            this.timer.Interval = TimeSpan.FromSeconds(1);
            this.timer.Tick += this.OnTimer;

            this.mediaPlayer.MediaOpened += this.OnMediaOpened;
        }

        public ICommand Play { get; set; }

        public ICommand Pause { get; set; }

        public ICommand Previous { get; set; }

        public ICommand Next { get; set; }

        public ObservableCollection<string> Songs { get; set; } = new ObservableCollection<string>();

        public string Elapsed
        {
            get
            {
                return this.elapsed;
            }
            set
            {
                this.Set(() => this.Elapsed, ref this.elapsed, value);
            }
        }

        public string Length
        {
            get
            {
                return this.length;
            }
            set
            {
                this.Set(() => this.Length, ref this.length, value);
            }
        }

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

                if (value != null)
                {
                    this.mediaPlayer.Open(new Uri(value));
                    if (this.isPlaying)
                    {
                        this.mediaPlayer.Play();
                    }
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

        private void OnTimer(object sender, EventArgs e)
        {
            this.Elapsed = this.mediaPlayer.Position.ToString(@"m\:ss");
        }

        private void OnMediaOpened(object sender, EventArgs e)
        {
            this.timer.Start();
            this.Length = this.mediaPlayer.NaturalDuration.TimeSpan.ToString(@"m\:ss");
            this.Elapsed = "0:00";
        }
    }
}