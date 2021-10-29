using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DayZTediratorToolz.Helpers;
using DayZTediratorToolz.Models;
using DayZTediratorToolz.Services;
using MaterialDesignThemes.Wpf;
using PropertyChanged;

namespace DayZTediratorToolz.Views
{
    [AddINotifyPropertyChangedInterface]
    public partial class AdminPanelView : BaseView
    {
        private readonly IAppSettingsManager _appSettingsManager;
        private readonly IServerInspectionService _serverInspectionService;
        private readonly INotificationService _notificationService;

        public DzsaLauncherApiResults.ServerInfo GameInfo { get; set; }

        private string collapsedStringValue;
        private bool collapsedBoolValue;
        private string _gameInfoIP;
        private string _gameInfoPort;
        private bool _canRefresh;

        public override ViewMenuData ViewMenuData { get; set; }

        public string IpDisplayValue { get => _gameInfoIP ?? fallbackIP; set => _gameInfoIP = value;}

        public string PortDisplayValue { get => _gameInfoPort ?? fallbackPORT; set => _gameInfoPort = value;}

        public bool CanRefresh { get => _canRefresh; set => _canRefresh = value;}

        public string ModCount { get => GameInfo?.Mods?.Count.ToString() ?? 0.ToString(); set => collapsedStringValue = value;}
        public bool IsBusy { get => !CanRefresh;}

        public ICommand NavToMod => new RelayCommand(o =>
        {
            Process.Start($"{_appSettingsManager.GetModPagePath()}{(o as DzsaLauncherApiResults.Mod).SteamWorkshopId}");
        }, o => true);

        public string PlayerRatio
        {
            get => $"{GameInfo?.Players}/{GameInfo?.MaxPlayers}" ?? "0/0";
            set => collapsedStringValue = value;
        }

        public string fallbackIP { get; set; }
        public string fallbackPORT { get; set; }

        string ConnectionInfo { get => $"{IpDisplayValue}:{PortDisplayValue}";}

        public AdminPanelView(IAppSettingsManager appSettingsManager, IServerInspectionService serverInspectionService, INotificationService _notificationService)
        {
            ViewMenuData = new(){ViewIndex = 1, ViewLabel = "Admin Panel", ViewIcon = PackIconKind.Server, ViewType = DayZTediratorConstants.ViewTypes.Admin};

            _appSettingsManager = appSettingsManager;
            _serverInspectionService = serverInspectionService;

            SubscribeToServiceEvents();

            this._notificationService = _notificationService;
            fallbackIP = appSettingsManager.GetServerIp();
            fallbackPORT = appSettingsManager.GetServerPort();
            InitializeComponent();
            DataContext = this;

            _serverInspectionService.Initialize(_appSettingsManager, _notificationService);
        }

        private void SubscribeToServiceEvents()
        {
            _serverInspectionService.ServerInformationChanged += (s, e) =>
            {
                GameInfo = (e as GameInfoEventArgs)?.GameInfoBlob ?? null;
                if (GameInfo == null) return;
                _gameInfoPort = GameInfo.GamePort.ToString();
                _gameInfoIP = GameInfo.Endpoint.Ip;
            };

            _serverInspectionService.ServiceStateChanged += (s,e) =>
            {
                var state = (e as ServiceStateArgs)?.ServiceState ?? DayZTediratorConstants.ServiceStates.Unknown;
                CanRefresh = state == DayZTediratorConstants.ServiceStates.Standby;
            };
        }

        private async void AdminPanelView_OnLoaded(object sender, RoutedEventArgs e)
        {
            await TryLoadServerInfo();
        }

        private async Task TryLoadServerInfo()
        {
            if (_serverInspectionService.CanCheck)
                await GetServerInfoAsync();
            else
                await _serverInspectionService.UpdatePing(GameInfo);
        }

        private void CopyIpPortClicked(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(ConnectionInfo);
        }

        private async void RefreshServerInfo(object sender, RoutedEventArgs e)
        {
            await TryLoadServerInfo();
        }

        private async Task GetServerInfoAsync()
        {
            await _serverInspectionService.GetGameInfo();
        }

        private void LaunchGame(object sender, RoutedEventArgs e)
        {
            Process.Start($@"steam://connect/{ConnectionInfo}");
        }
    }
}