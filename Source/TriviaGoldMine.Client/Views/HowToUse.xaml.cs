namespace TriviaGoldMine.Client.Views
{
    using System.Windows;

    using Properties;

    public partial class HowToUse
    {
        public HowToUse()
        {
            this.InitializeComponent();
            this.checkBox.IsChecked = !Settings.Default.HowToUse;
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.HowToUse = false;
            Settings.Default.Save();
        }

        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.HowToUse = true;
            Settings.Default.Save();
        }
    }
}
