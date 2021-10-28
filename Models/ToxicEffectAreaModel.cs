using Newtonsoft.Json;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using DayZTediratorToolz.Helpers;

namespace DayZTediratorToolz.Models
{
    [AddINotifyPropertyChangedInterface]
    public class ToxicEffectAreaModel
    {
        /* #### Uncomment this line after you paste in the sub-classes generated from JSON #### */

        public Root RootObject { get; set; }

        /* ------------------------------------------ */

        /*  Replace 'ToxicEffectConfig' with the type generated from the object JSON;
            an example of this would be RootObject.Types in the TypeCollectionModel class
            where 'ToxicEffectConfig' would be replaced with 'Types'.

            Once you've replaced 'ToxicEffectConfig' uncomment the following methods:
        */

        public ToxicEffectConfig GetContent()
        {
            return RootObject?.ToxicEffectConfig ?? null;
        }

        public void Initialize(string json)
        {
            try
            {
                RootObject = JsonConvert.DeserializeObject<Root>(json, new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore
                });
                InitSafePositionCollection();
            }
            catch (Exception e)
            {
                throw new Exception("There was an error attempting to initialize the ToxicEffectConfig Data.");
            }
        }

        public void InitSafePositionCollection()
        {
            foreach (var sPos in RootObject.ToxicEffectConfig.SafePositions)
            {
                if (sPos.Length < 2)
                    break;

                RootObject.ToxicEffectConfig.SafePositionCollection.Add(new SafePosMapCoordinate(sPos[0], sPos[1]));
            }
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(RootObject);
        }

        /*  ------------------------------------------ */

        // PASTE CLASS CODE GENERATED FROM JSON HERE

        // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
        [AddINotifyPropertyChangedInterface]
        public class Data
        {
            [JsonProperty("Pos")]
            public ObservableCollection<int> Pos { get; set; }

            [JsonProperty("Radius")]
            public int Radius { get; set; }

            [JsonProperty("PosHeight")]
            public int PosHeight { get; set; }

            [JsonProperty("NegHeight")]
            public int NegHeight { get; set; }

            [JsonProperty("InnerRingCount")]
            public int InnerRingCount { get; set; }

            [JsonProperty("InnerPartDist")]
            public int InnerPartDist { get; set; }

            [JsonProperty("OuterRingToggle")]
            public int OuterRingToggle { get; set; }

            [JsonProperty("OuterPartDist")]
            public int OuterPartDist { get; set; }

            [JsonProperty("OuterOffset")]
            public int OuterOffset { get; set; }

            [JsonProperty("VerticalLayers")]
            public int VerticalLayers { get; set; }

            [JsonProperty("VerticalOffset")]
            public int VerticalOffset { get; set; }

            [JsonProperty("ParticleName")]
            public string ParticleName { get; set; }
        }

        [AddINotifyPropertyChangedInterface]
        public class PlayerData
        {
            [JsonProperty("AroundPartName")]
            public string AroundPartName { get; set; }

            [JsonProperty("TinyPartName")]
            public string TinyPartName { get; set; }

            [JsonProperty("PPERequesterType")]
            public string PPERequesterType { get; set; }
        }

        [AddINotifyPropertyChangedInterface]
        public class Area
        {
            [JsonProperty("AreaName")]
            public string AreaName { get; set; }

            [JsonProperty("Type")]
            public string Type { get; set; }

            [JsonProperty("TriggerType")]
            public string TriggerType { get; set; }

            [JsonProperty("Data")]
            public Data Data { get; set; }

            [JsonProperty("PlayerData")]
            public PlayerData PlayerData { get; set; }
        }

        [AddINotifyPropertyChangedInterface]
        public class ToxicEffectConfig
        {
            [JsonProperty("Areas")]
            public ObservableCollection<Area> Areas { get; set; }

            [JsonProperty("SafePositions")]
            public List<int[]> SafePositions { get; set; }

            public ObservableCollection<SafePosMapCoordinate> SafePositionCollection { get; set; }
        }

        [AddINotifyPropertyChangedInterface]
        public class Root
        {
            [JsonProperty("ToxicEffectConfig")]
            public ToxicEffectConfig ToxicEffectConfig { get; set; }
        }



        // PASTE CLASS CODE GENERATED FROM JSON HERE
    }
}
