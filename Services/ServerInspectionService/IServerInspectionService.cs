using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PropertyChanged;
using SteamQueryNet;
using SteamQueryNet.Interfaces;
using SteamQueryNet.Models;

namespace DayZTediratorToolz.Services
{
    public interface IServerInspectionService
    {
        void Initialize(IAppSettingsManager settingsManager, INotificationService _notificationService);
        Task<GameInfoBlob> GetGameInfo();
        void ShutDownInspectionService();
    }
    
    [AddINotifyPropertyChangedInterface]
    public class ServerInspectionService : IServerInspectionService
    {
        private INotificationService notificationService;
        IServerQuery GameServerQuery { get; set; }
        ServerInfo GameServerInfo { get; set; }
        List<Player> GamePlayers { get; set; }
        GameInfoBlob GameInfo { get; set; }

        private (string IP, string Port) ServerConnectionInfo { get; set; }

        public void Initialize(IAppSettingsManager settingsManager, INotificationService _notificationService)
        {
            notificationService = _notificationService;
            ServerConnectionInfo = (settingsManager.GetServerIp(), settingsManager.GetServerPort());
            var serverAddressAndPort = $"{ServerConnectionInfo.IP}:{ServerConnectionInfo.Port}";
            GameServerQuery = new ServerQuery(serverAddressAndPort);
        }

        public async Task<GameInfoBlob> GetGameInfo()
        {
            try
            {
                GameServerInfo = await GameServerQuery.GetServerInfoAsync();
                GamePlayers = await GameServerQuery.GetPlayersAsync();
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
                };
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
            var gameServerQuery = (GameServerQuery as ServerQuery);
            if(gameServerQuery.IsConnected)
                gameServerQuery.Dispose();
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