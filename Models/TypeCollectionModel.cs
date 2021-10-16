using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PropertyChanged;

namespace DayZTediratorToolz.Models
{
    public class ITypeSubCollection
    {
        string Name { get; set; }
    }
    
    
    [AddINotifyPropertyChangedInterface]
    public class TypeCollectionModel
    {
        
        public Root RootObj { get; set; }

        public Types GetTypeCollection()
        {
            if (RootObj == null)
                return null;
            
            return RootObj.Types;
        }
        
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

            foreach (var type in this.RootObj.Types.Type)
            {
                if (type.Usages == null)
                    type.Usages = new ObservableCollection<Usage>();
                
                if (type.Tiers == null)
                    type.Tiers = new ObservableCollection<Value>();

                if (type.Category == null)
                    type.Category = new Category();
            }
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(RootObj);
        }


        [AddINotifyPropertyChangedInterface]
        public class Flags
        {
            [JsonProperty("@count_in_cargo")]
            public string CountInCargo { get; set; }

            [JsonProperty("@count_in_hoarder")]
            public string CountInHoarder { get; set; }

            [JsonProperty("@count_in_map")]
            public string CountInMap { get; set; }

            [JsonProperty("@count_in_player")]
            public string CountInPlayer { get; set; }

            [JsonProperty("@crafted")]
            public string Crafted { get; set; }

            [JsonProperty("@deloot")]
            public string Deloot { get; set; }
        }

        [AddINotifyPropertyChangedInterface]
        public class Category
        {
            [JsonProperty("@name")]
            public string Name { get; set; }
        }

        [AddINotifyPropertyChangedInterface]
        public class Usage : ITypeSubCollection
        {
            [JsonProperty("@name")]
            public string Name { get; set; }
        }

        [AddINotifyPropertyChangedInterface]
        public class Value : ITypeSubCollection
        {
            [JsonProperty("@name")]
            public string Name { get; set; }
        }

        [AddINotifyPropertyChangedInterface]
        public class Type
        {
            [JsonIgnore] public string UID { get; set; } = Guid.NewGuid().ToString().Split('-')[0];
            
            [JsonProperty("@name")]
            public string Name { get; set; }

            [JsonProperty("nominal")]
            public int Nominal { get; set; }

            [JsonProperty("lifetime")]
            public int Lifetime { get; set; }

            [JsonProperty("restock")]
            public int Restock { get; set; }

            [JsonProperty("min")]
            public int Min { get; set; }

            [JsonProperty("quantmin")]
            public int Quantmin { get; set; }

            [JsonProperty("quantmax")]
            public int Quantmax { get; set; }

            [JsonProperty("cost")]
            public int Cost { get; set; }

            [JsonProperty("flags")]
            public Flags Flags { get; set; }

            [JsonProperty("category")]
            public Category Category { get; set; }
            
            [JsonIgnore]
            public string CategoryRef { get => Category.Name; }
            
            [JsonIgnore]
            public string UsagesCountMsg { get => $"#: {Usages.Count.ToString()}"; }
            
            [JsonIgnore]
            public string TiersCountMsg { get => $"#: {Tiers.Count.ToString()}"; }

            [JsonProperty("usage")]
            public ObservableCollection<Usage> Usages { get; set; }

            [JsonProperty("value")]
            public ObservableCollection<Value> Tiers { get; set; }
        }


        [AddINotifyPropertyChangedInterface]
        public class Types
        {
            [JsonProperty("type")]
            public ObservableCollection<Type> Type { get; set; }
        }

        [AddINotifyPropertyChangedInterface]
        public class Root
        {
            [JsonProperty("types")]
            public Types Types { get; set; }
        }

    }
}