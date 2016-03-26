namespace TriviaGoldMine.Client.Views
{
    using System.IO;
    using System.Linq;
    using System.Windows;

    using ViewModels;

    public partial class Questions
    {
        public Questions()
        {
            this.InitializeComponent();
        }

        private void Questions_OnDrop(object sender, DragEventArgs e)
        {
            var droppedFilenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            (this.DataContext as QuestionsViewModel).ParseSpreadsheet(droppedFilenames.First());
        }

        private void Questions_OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                foreach (string filename in filenames)
                {
                    if (Path.GetExtension(filename)?.ToLower() != ".xlsx")
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
