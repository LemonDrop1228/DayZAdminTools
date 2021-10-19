using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DayZTediratorToolz.Helpers;
using DayZTediratorToolz.Models;
using DayZTediratorToolz.Services;
using PropertyChanged;

namespace DayZTediratorToolz.Views.AdminPanel
{
    [AddINotifyPropertyChangedInterface]
    public partial class AdminPanelView : BaseView
    {
        private readonly IAppSettingsManager _appSettingsManager;
        private readonly IServerInspectionService _serverInspectionService;
        private readonly INotificationService _notificationService;

        public DzsaLauncherApiResults.ServerInfo GameInfo { get; set; }
        
        private string collapsedValue;
        
        public string IpDisplayValue { get => GameInfo?.Endpoint.Ip ?? fallbackIP; set => collapsedValue = value;}
        
        public string PortDisplayValue { get => GameInfo?.GamePort.ToString() ?? fallbackPORT; set => collapsedValue = value;}

        public string ModCount { get => GameInfo?.Mods?.Count.ToString() ?? 0.ToString(); set => collapsedValue = value;}

        
        public ICommand NavToMod => new RelayCommand(o =>
        {
            Process.Start($"{_appSettingsManager.GetModPagePath()}{(o as DzsaLauncherApiResults.Mod).SteamWorkshopId}");
        }, o => true);
        
        public string PlayerRatio
        {
            get => $"{GameInfo?.Players}/{GameInfo?.MaxPlayers}" ?? "0/0";
            set => collapsedValue = value;
        }

        public string fallbackIP { get; set; }
        public string fallbackPORT { get; set; }

        string ConnectionInfo { get => $"{IpDisplayValue}:{PortDisplayValue}";}

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