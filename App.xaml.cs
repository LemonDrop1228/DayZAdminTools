using System.Windows;
using DayZTediratorToolz.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Windows.Documents;
using DayZTediratorToolz.Views;
using DayZTediratorToolz.Views.AdminPanel;
using DayZTediratorToolz.Views.Types;

namespace DayZTediratorToolz
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private readonly IHost host;

        public App()
        {
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NTA0ODQwQDMxMzkyZTMyMmUzMGVyRWxHSFFkYWFmeDVRNzFwZmhSblliNFpBc1NDblZlRVJvWXJPWmxNMEU9");
            
            host = new HostBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IAppSettingsManager>(provider => new AppSettingsManager(ConfigurationManager.AppSettings));
                    services.AddSingleton<IServerInspectionService>(provider => new ServerInspectionService());
                    services.AddSingleton<IControllerService>(provider => new ControllerService());
                    services.AddSingleton<ITypesConvertorService>(provider => new TypesConvertorService());
                    services.AddSingleton<IGeneralHelperService>(provider => new GeneralHelperService());
                    services.AddSingleton<INotificationService>(provider => new NotificationService());
                    
                    services.AddSingleton<HomeView>();
                    services.AddSingleton<AdminPanelView>();
                    services.AddSingleton<TypesEditorView>();
                    services.AddSingleton<MainWindow>();
                }).Build();
        }
        
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            
            var mainWindow = host.Services.GetService<MainWindow>();
            host.Services.GetService<IControllerService>().InitializeViews(
                host.Services.GetService<HomeView>(),
                host.Services.GetService<AdminPanelView>(),
                host.Services.GetService<TypesEditorView>()
            );

            mainWindow.Closed += (s,e) => {
                ShutItDown();
            };
            
            mainWindow.Show();
        }
        
        private void ShutItDown()
        {
            using (host)
            {
                host.StopAsync();
            }

            Current.Shutdown();
        }
    }
}