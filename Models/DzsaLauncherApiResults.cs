using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PropertyChanged;

namespace DayZTediratorToolz.Models
{
    [AddINotifyPropertyChangedInterface]
    public class DzsaLauncherApiResults
    {
        public Root RootObj { get; set; }
        
        public void Initialize(string json)
        {
            try
            {
                RootObj = JsonConvert.DeserializeObject<Root>(json, new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });
            }
            catch (Exception e)
            {
                throw new Exception("There was an error attempting to initialize the Types Data.");
            }
        }
        
        [AddINotifyPropertyChangedInterface]
    public class Mod
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("steamWorkshopId")]
        public object SteamWorkshopId { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class Endpoint
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("port")]
        public int Port { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class ServerInfo
    {
        public static ServerInfo CreateInstance(ServerInfo serverInfo)
        {
            return new ServerInfo(
                serverInfo.Sponsor,
                serverInfo.Profile ,
                serverInfo.NameOverride ,
                serverInfo.Mods ,
                serverInfo.Game ,
                serverInfo.Endpoint ,
                serverInfo.Name ,
                serverInfo.Map ,
                serverInfo.Mission ,
                serverInfo.Players ,
                serverInfo.MaxPlayers ,
                serverInfo.Environment ,
                serverInfo.Password ,
                serverInfo.Version ,
                serverInfo.Vac ,
                serverInfo.GamePort ,
                serverInfo.Shard ,
                serverInfo.BattlEye ,
                serverInfo.TimeAcceleration ,
                serverInfo.Time ,
                serverInfo.FirstPersonOnly 
                );
        }
        
        public ServerInfo(bool sponsor, bool profile, bool nameOverride, List<Mod> mods, string game, Endpoint endpoint, string name, string map, string mission, int players, int maxPlayers, string environment, bool password, string version, bool vac, int gamePort, string shard, bool battlEye, int timeAcceleration, string time, bool firstPersonOnly)
        {
            Sponsor = sponsor;
            Profile = profile;
            NameOverride = nameOverride;
            Mods = mods;
            Game = game;
            Endpoint = endpoint;
            Name = name;
            Map = map;
            Mission = mission;
            Players = players;
            MaxPlayers = maxPlayers;
            Environment = environment;
            Password = password;
            Version = version;
            Vac = vac;
            GamePort = gamePort;
            Shard = shard;
            BattlEye = battlEye;
            TimeAcceleration = timeAcceleration;
            Time = time;
            FirstPersonOnly = firstPersonOnly;
        }
       
        [JsonIgnore]
        public int Ping { get; set; }

        [JsonProperty("sponsor")]
        public bool Sponsor { get; set; }

        [JsonProperty("profile")]
        public bool Profile { get; set; }

        [JsonProperty("nameOverride")]
        public bool NameOverride { get; set; }

        [JsonProperty("mods")]
        public List<Mod> Mods { get; set; }

        [JsonProperty("game")]
        public string Game { get; set; }

        [JsonProperty("endpoint")]
        public Endpoint Endpoint { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("map")]
        public string Map { get; set; }

        [JsonProperty("mission")]
        public string Mission { get; set; }

        [JsonProperty("players")]
        public int Players { get; set; }

        [JsonProperty("maxPlayers")]
        public int MaxPlayers { get; set; }

        [JsonProperty("environment")]
        public string Environment { get; set; }

        [JsonProperty("password")]
        public bool Password { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("vac")]
        public bool Vac { get; set; }

        [JsonProperty("gamePort")]
        public int GamePort { get; set; }

        [JsonProperty("shard")]
        public string Shard { get; set; }

        [JsonProperty("battlEye")]
        public bool BattlEye { get; set; }

        [JsonProperty("timeAcceleration")]
        public int TimeAcceleration { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("firstPersonOnly")]
        public bool FirstPersonOnly { get; set; }
    }

    [AddINotifyPropertyChangedInterface]
    public class Root
    {
        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("serverInfo")]
        public List<ServerInfo> ServerInfo { get; set; }
    }
    }

}