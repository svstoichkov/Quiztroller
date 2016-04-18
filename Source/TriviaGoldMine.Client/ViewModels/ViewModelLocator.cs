namespace Quiztroller.ViewModels
{
    using Autofac;

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            this.Register();
        }

        public static IContainer Container { get; private set; }

        public MainViewModel MainViewModel => Container.Resolve<MainViewModel>();

        public Mp3PlayerViewModel Mp3PlayerViewModel => Container.Resolve<Mp3PlayerViewModel>();

        public PowerPointControllerViewModel PowerPointControllerViewModel => Container.Resolve<PowerPointControllerViewModel>();

        public QuestionsViewModel QuestionsViewModel => Container.Resolve<QuestionsViewModel>();

        public ScoreboardViewModel ScoreboardViewModel => Container.Resolve<ScoreboardViewModel>();

        private void Register()
        {
            var containerBuilder = new ContainerBuilder();
            
            containerBuilder.RegisterType<MainViewModel>().SingleInstance();
            containerBuilder.RegisterType<Mp3PlayerViewModel>().SingleInstance();
            containerBuilder.RegisterType<PowerPointControllerViewModel>().SingleInstance();
            containerBuilder.RegisterType<QuestionsViewModel>().SingleInstance();
            containerBuilder.RegisterType<ScoreboardViewModel>().SingleInstance();

            Container = containerBuilder.Build();
        }
    }
}