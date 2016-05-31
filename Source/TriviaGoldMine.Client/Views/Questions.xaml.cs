namespace Quiztroller.Views
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
            (this.DataContext as QuestionsViewModel).OpenQuizPackage(droppedFilenames.First());
        }

        private void Questions_OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filenames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                if (Path.GetExtension(filenames.First()) != ".qz")
                {
                    e.Effects = DragDropEffects.None;
                    e.Handled = true;
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