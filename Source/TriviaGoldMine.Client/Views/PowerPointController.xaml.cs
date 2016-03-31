namespace Quiztroller.Views
{
    using System.IO;
    using System.Linq;
    using System.Windows;

    using ViewModels;

    public partial class PowerPointController
    {
        public PowerPointController()
        {
            this.InitializeComponent();
        }

        private void PowerPoint_OnDrop(object sender, DragEventArgs e)
        {
            var filename = (e.Data.GetData(DataFormats.FileDrop, true) as string[]).First();
            ((PowerPointControllerViewModel)this.DataContext).AddFilePath(filename);
        }

        private void PowerPoint_OnDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var filename = (e.Data.GetData(DataFormats.FileDrop, true) as string[]).First();

                if (Path.GetExtension(filename)?.ToLower() != ((PowerPointControllerViewModel)this.DataContext).AcceptedExtension)
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
