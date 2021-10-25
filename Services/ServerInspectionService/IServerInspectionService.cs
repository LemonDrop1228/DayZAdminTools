using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using DayZTediratorToolz.Helpers;
using DayZTediratorToolz.Models;
using PropertyChanged;

namespace DayZTediratorToolz.Services
{
    public interface IServerInspectionService
    {
        void Initialize(IAppSettingsManager settingsManager, INotificationService _notificationService);
        Task GetGameInfo();
        void ShutDownInspectionService();
        bool CanCheck { get; }
        bool IsBusy { get; set; }
        event EventHandler ServiceStateChanged;
        Task UpdatePing(DzsaLauncherApiResults.ServerInfo gameInfo);
        event EventHandler ServerInformationChanged;
    }
    
    [AddINotifyPropertyChangedInterface]
    public class ServerInspectionService : IServerInspectionService
    {
        public event EventHandler ServerInformationChanged;
        public event EventHandler ServiceStateChanged;
        
        private INotificationService notificationService;
        private DayZTediratorConstants.ServiceStates _serviceState;
        Models.DzsaLauncherApiResults.ServerInfo GameServerInfo { get; set; }
        DzsaLauncherApiResults.ServerInfo GameInfo { get; set; }
        private DateTimeOffset lastCheckTimeOffset { get; set; }

        private DayZTediratorConstants.ServiceStates State
        {
            get => _serviceState;
            set
            {
                if (_serviceState != value)
                {
                    _serviceState = value;
                    SendStateChangeEvent(_serviceState);
                }
            }
        }

        public bool CanCheck { get => CanCheckServerInfo(); }
        public bool IsBusy { get; set; }


        private (string QueryUri, string IP, string Port) ServerConnectionInfo { get; set; }

        public void Initialize(IAppSettingsManager settingsManager, INotificationService _notificationService)
        {
            notificationService = _notificationService;
            ServerConnectionInfo = (settingsManager.GetQueryUri(), settingsManager.GetServerIp(), settingsManager.GetServerPort());
            State = DayZTediratorConstants.ServiceStates.Standby;
        }
        
        public void ShutDownInspectionService()
        {
            
        }

        public async Task GetGameInfo()
        {
            State = DayZTediratorConstants.ServiceStates.Busy;
            try
            {
                var jsonResultStr = string.Empty;

                using (var wc = new WebClient())
                {
                    jsonResultStr = await wc.DownloadStringTaskAsync(new Uri(ServerConnectionInfo.QueryUri));
                }

                if (jsonResultStr.NullOrEmpty())
                    throw new Exception();

                DzsaLauncherApiResults apiResults = new DzsaLauncherApiResults();

                apiResults.Initialize(jsonResultStr.Replace("result", "serverInfo"));

                GameInfo = apiResults.RootObj.ServerInfo
                    .Where(s => (s.Endpoint.Ip, s.GamePort.ToString()) ==
                                (ServerConnectionInfo.IP, ServerConnectionInfo.Port)).Select(s =>
                        Models.DzsaLauncherApiResults.ServerInfo.CreateInstance(serverInfo: s)).FirstOrDefault();

                GameInfo.Ping = await GetServerPing();

                apiResults.RootObj.ServerInfo.Clear();
                apiResults = null;
            }
            catch (Exception e)
            {
                notificationService.SetNotificationContent("Server offline", "Unable to collect server information.");
                notificationService.NotifyFailure();
                State = DayZTediratorConstants.ServiceStates.Failed;
            }
            finally
            {
                lastCheckTimeOffset = DateTimeOffset.Now;
                if (State != DayZTediratorConstants.ServiceStates.Standby)
                {
                    SendChangeEvent();
                    State = DayZTediratorConstants.ServiceStates.Standby;
                }
            }
        }

        private void SendChangeEvent()
        {
            if (ServerInformationChanged != null)
                ServerInformationChanged(this,new GameInfoEventArgs(){GameInfoBlob = GameInfo});
        }

        private void SendStateChangeEvent(DayZTediratorConstants.ServiceStates state)
        {
            if (ServiceStateChanged != null)
                ServiceStateChanged(this, new ServiceStateArgs() { ServiceState = state });
        }

        public async Task UpdatePing(DzsaLauncherApiResults.ServerInfo gameInfo)
        {
            IsBusy = true;
            gameInfo.Ping = await GetServerPing(true);
        }

        private async Task<int> GetServerPing(bool markNotBusy = false)
        {
            var serverPing = 0;
                
            using (var p = new Ping())
            {
                var reply = await p.SendPingAsync(ServerConnectionInfo.IP);
                if (reply.Status == IPStatus.Success)
                    serverPing = Convert.ToInt32(reply.RoundtripTime);
            }

            if (markNotBusy)
                IsBusy = false;
            
            SendChangeEvent();
            return serverPing;
        }

        private bool CanCheckServerInfo()
        {
            if (lastCheckTimeOffset == DateTimeOffset.MinValue)
                return true;

            var checkTimeOffset = (lastCheckTimeOffset - DateTimeOffset.Now);
            var canCheckServerInfo = (((checkTimeOffset.Minutes * 60) + (checkTimeOffset.Seconds)) * -1) > 60;
            return canCheckServerInfo;
        }
    }

    public class GameInfoEventArgs : EventArgs
    {
        public DzsaLauncherApiResults.ServerInfo GameInfoBlob { get; set; }
    }

    public class ServiceStateArgs : EventArgs
    {
        public DayZTediratorConstants.ServiceStates ServiceState { get; set; }
    }
}