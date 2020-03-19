using Prism.Ioc;
using MReader.Core.Views;
using System.Windows;
using MReader.Core.Services;

namespace MReader.Core
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IFileService, FileService>();
            containerRegistry.Register<ISettingsService, SettingsService>();
            containerRegistry.Register<ILoggingService, LoggingService>();
        }
    }
}
