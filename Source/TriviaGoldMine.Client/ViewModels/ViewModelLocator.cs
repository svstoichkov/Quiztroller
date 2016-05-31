namespace Quiztroller.ViewModels
{
    using Autofac;

    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            if (Container == null)
            {
                Register();
            }
        }

        public static IContainer Container { get; private set; }

        public MainViewModel MainViewModel => Container.Resolve<MainViewModel>();

        public Mp3PlayerViewModel Mp3PlayerViewModel => Container.Resolve<Mp3PlayerViewModel>();

        public PowerPointControllerViewModel PowerPointControllerViewModel => Container.Resolve<PowerPointControllerViewModel>();

        public QuestionsViewModel QuestionsViewModel => Container.Resolve<QuestionsViewModel>();

        public ScoreboardViewModel ScoreboardViewModel => Container.Resolve<ScoreboardViewModel>();

        public LoginViewModel LoginViewModel => Container.Resolve<LoginViewModel>();

        public static void Register()
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterType<MainViewModel>().SingleInstance();
            containerBuilder.RegisterType<Mp3PlayerViewModel>().SingleInstance();
            containerBuilder.RegisterType<PowerPointControllerViewModel>().SingleInstance();
            containerBuilder.RegisterType<QuestionsViewModel>().SingleInstance();
            containerBuilder.RegisterType<ScoreboardViewModel>().SingleInstance();
            containerBuilder.RegisterType<LoginViewModel>().SingleInstance();

            Container = containerBuilder.Build();
        }
    }
}