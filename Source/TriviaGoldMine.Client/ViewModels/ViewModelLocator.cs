namespace TriviaGoldMine.Client.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel { get; set; } = new MainViewModel();

        public Mp3PlayerViewModel Mp3PlayerViewModel { get; set; } = new Mp3PlayerViewModel();

        public PowerPointControllerViewModel PowerPointControllerViewModel { get; set; } = new PowerPointControllerViewModel();
    }
}
