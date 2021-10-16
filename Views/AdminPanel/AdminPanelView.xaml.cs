using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using DayZTediratorToolz.Services;

namespace DayZTediratorToolz.Views.AdminPanel
{
    public partial class AdminPanelView : BaseView
    {
        private readonly IAppSettingsManager _appSettingsManager;
        private readonly IServerInspectionService _serverInspectionService;
        private readonly INotificationService _notificationService;

        public GameInfoBlob GameInfo { get; set; }
        private string collapsedValue;

        public string IpDisplayValue { get => GameInfo?.IPAddr ?? fallbackIP; set => collapsedValue = value;}
        public string PortDisplayValue { get => GameInfo?.Port ?? fallbackPORT; set => collapsedValue = value;}

        public string fallbackIP { get; set; }
        public string fallbackPORT { get; set; }

        string ConnectionInfo { get => $"{GameInfo.IPAddr}:{GameInfo.Port}";}

        public AdminPanelView(IAppSettingsManager appSettingsManager, IServerInspectionService serverInspectionService, INotificationService _notificationService)
        {
            _appSettingsManager = appSettingsManager;
            _serverInspectionService = serverInspectionService;
            this._notificationService = _notificationService;
            fallbackIP = appSettingsManager.GetServerIp();
            fallbackPORT = appSettingsManager.GetServerPort();
            InitializeComponent();
            DataContext = this;
            
            _serverInspectionService.Initialize(_appSettingsManager, _notificationService);
        }

        private async void AdminPanelView_OnLoaded(object sender, RoutedEventArgs e)
        {
            await GetServerInfoAsync();
        }

        private void CopyIpPortClicked(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ConnectionInfo);
        }

        private async void RefreshServerInfo(object sender, RoutedEventArgs e)
        {
            await GetServerInfoAsync();
        }

        private async Task GetServerInfoAsync()
        {
            GameInfo = await _serverInspectionService.GetGameInfo();
        }

        private void LaunchGame(object sender, RoutedEventArgs e)
        {
            Process.Start($@"steam://connect/{ConnectionInfo}");
        }
    }
}