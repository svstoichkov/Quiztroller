namespace Quiztroller
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;

    using Autofac;

    using Properties;

    using ViewModels;

    public partial class App
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            string fname = null;
            if (AppDomain.CurrentDomain.SetupInformation?.ActivationArguments?.ActivationData != null && AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData.Length > 0)
            {
                try
                {
                    fname = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData[0];
                    
                    var uri = new Uri(fname);
                    fname = uri.LocalPath;

                    this.Properties["ArbitraryArgName"] = fname;

                }
                catch (Exception ex)
                {
                }
            }
            
            if (fname != null)
            {
                ViewModelLocator.Register();
                ViewModelLocator.Container.Resolve<QuestionsViewModel>().OpenQuizPackage(fname);

                if (Settings.Default.HowToUse)
                {
                    ViewModelLocator.Container.Resolve<MainViewModel>().Switch.Execute(null);
                }
            }

            new MainWindow().Show();
        }
    }
}