using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Controls;
using DayZTediratorToolz.Helpers;
using DayZTediratorToolz.Models;
using PropertyChanged;

namespace DayZTediratorToolz.Services
{
    public interface IServerInspectionService
    {
        void Initialize(IAppSettingsManager settingsManager, INotificationService _notificationService);
        Task<DzsaLauncherApiResults.ServerInfo> GetGameInfo();
        void ShutDownInspectionService();
    }
    
    [AddINotifyPropertyChangedInterface]
    public class ServerInspectionService : IServerInspectionService
    {
        private INotificationService notificationService;
        Models.DzsaLauncherApiResults.ServerInfo GameServerInfo { get; set; }
        DzsaLauncherApiResults.ServerInfo GameInfo { get; set; }


        private (string QueryUri, string IP, string Port) ServerConnectionInfo { get; set; }

        public void Initialize(IAppSettingsManager settingsManager, INotificationService _notificationService)
        {
            notificationService = _notificationService;
            ServerConnectionInfo = (settingsManager.GetQueryUri(), settingsManager.GetServerIp(), settingsManager.GetServerPort());
            
        }

        public async Task<DzsaLauncherApiResults.ServerInfo> GetGameInfo()
        {
            try
            {
                var jsonResultStr = string.Empty;
                
                using (var wc = new WebClient())
                {
                    jsonResultStr = await wc.DownloadStringTaskAsync(new Uri(ServerConnectionInfo.QueryUri));
                }

                var serverPing = 0;
                
                using (var p = new Ping())
                {
                    var reply = await p.SendPingAsync(ServerConnectionInfo.IP);
                    if (reply.Status == IPStatus.Success)
                        serverPing = Convert.ToInt32(reply.RoundtripTime);
                }
                
                
                if (jsonResultStr.NullOrEmpty())
                    throw new Exception();

                DzsaLauncherApiResults apiResults = new DzsaLauncherApiResults();
                
                apiResults.Initialize(jsonResultStr.Replace("result", "serverInfo"));

                GameInfo = apiResults.RootObj.ServerInfo.
                    Where(s => (s.Endpoint.Ip, s.GamePort.ToString()) == (ServerConnectionInfo.IP, ServerConnectionInfo.Port)).
                    Select(s => Models.DzsaLauncherApiResults.ServerInfo.CreateInstance(serverInfo: s)).
                    FirstOrDefault();

                GameInfo.Ping = serverPing;

                apiResults.RootObj.ServerInfo.Clear();
                apiResults = null;

                #region SteamQueryNet server info logic --- Doesn't work currently

                /*GameServerQuery.Connect(ServerConnectionInfo.IP, Convert.ToUInt16(ServerConnectionInfo.Port));
                GamePlayers = await GameServerQuery.GetPlayersAsync();
                GameServerInfo = await GameServerQuery.GetServerInfoAsync();
                GameInfo = new GameInfoBlob()
                {
                    IPAddr = ServerConnectionInfo.IP,
                    Port = ServerConnectionInfo.Port,
                    ServerName = GameServerInfo.Name,
                    MapName = GameServerInfo.Map,
                    PlayerRatio = $"{GameServerInfo.Players}/{GameServerInfo.MaxPlayers}",
                    Version = GameServerInfo.Version,
                    Keywords = GameServerInfo.Keywords,
                    Ping = GameServerInfo.Ping.ToString()
                };*/

                #endregion
            }
            catch (Exception e)
            {
                notificationService.SetNotificationContent("Server offline","Unable to collect server information.");
                notificationService.NotifyFailure();
                return GameInfo;
            }
            
            return GameInfo;
        }
        

        public void ShutDownInspectionService()
        {
            /*var gameServerQuery = (GameServerQuery as ServerQuery);
            if(gameServerQuery.IsConnected)
                gameServerQuery.Dispose();*/
        }

    }

    public record GameInfoBlob
    {
        public string ServerName { get; set; }
        public string IPAddr { get; set; }
        public string Port { get; set; }
        public string PlayerRatio { get; set; }
        public string MapName { get; set; }
        public string Version { get; set; }
        public string Keywords { get; set; }
        public string Ping { get; set; }
    }
}