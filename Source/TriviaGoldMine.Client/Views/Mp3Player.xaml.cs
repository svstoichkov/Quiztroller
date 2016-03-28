namespace TriviaGoldMine.Client.Views
{
    using System.IO;
    using System.Linq;
    using System.Windows;

    using ViewModels;

    public partial class Mp3Player
    {
        public Mp3Player()
        {
            this.InitializeComponent();
        }

        private void Mp3Player_OnDrop(object sender, DragEventArgs e)
        {
            var droppedFilenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

            if (Directory.Exists(droppedFilenames.First()))
            {
                droppedFilenames = Directory.GetFiles(droppedFilenames.First());
            }

            (this.DataContext as Mp3PlayerViewModel).AddSongs(droppedFilenames);
        }

        private void Mp3Player_OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                if (Directory.Exists(filenames.First()))
                {
                    filenames = Directory.GetFiles(filenames.First());
                }

                foreach (string filename in filenames)
                {
                    if (Path.GetExtension(filename)?.ToLower() != ".mp3")
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = true;
                        break;
                    }
                }
            }
            else
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }
    }
}
