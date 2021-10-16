using System;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using DayZTediratorToolz.Models;
using Newtonsoft.Json;

namespace DayZTediratorToolz.Services
{
    public interface ITypesConvertorService
    {
        void SetInitData(string _xml);
        Task InitializeTypes();
        bool TypesLoadedSuccesfully();
        TypeCollectionModel.Types GetTypes();
        Task<string> GetSerializedTypesXml();
        void ResetTypesCollection();
    }

    public class TypesConvertorService : ITypesConvertorService
    {
        TypeCollectionModel TypeCollection { get; set; }
        string InitializingJsonData { get; set; }
        string TypesXMLData { get; set; }

        private bool ContainsSyntaxErrors { get; set; }

        public async Task InitializeTypes()
        {
            TypeCollection = new TypeCollectionModel();
            await ConvertXMLtoJSON();
            if(!ContainsSyntaxErrors)
                TypeCollection.Initialize(InitializingJsonData);
        }

        public bool TypesLoadedSuccesfully()
        {
            return !string.IsNullOrEmpty(InitializingJsonData);
        }

        public void SetInitData(string _xml)
        {
            TypesXMLData = _xml;
        }
        
        private async Task ConvertXMLtoJSON()
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(TypesXMLData);
                ContainsSyntaxErrors = false;
            }
            catch (Exception e)
            {
                ContainsSyntaxErrors = true;
                return;
            }
            
            var cleanXDoc = await Task.Run( () => AttachJsonArrayAttribute(doc, "usage", "value", "type"));

            InitializingJsonData = JsonConvert.SerializeXmlNode(cleanXDoc.DocumentElement, Newtonsoft.Json.Formatting.Indented);
        }

        private XmlDocument AttachJsonArrayAttribute(XmlDocument doc, params string[] tags)
        {
            XmlDocument xDoc = null;
            foreach (var tag in tags)
            {
                var products = doc.GetElementsByTagName(tag);


                if (products.Count > 0)
                {
                    for (int i = 0; i < products.Count; i++)
                    {
                        var attribute = doc.CreateAttribute("json", "Array", "http://james.newtonking.com/projects/json");
                        attribute.InnerText = "true";
                        var node = products.Item(i) as XmlElement;
                        node.Attributes.Append(attribute);
                    }
                }
            }

            xDoc = doc;
            doc = null;
            return xDoc;
        }
        
        private XmlDocument DetachJsonArrayAttribute(XmlDocument doc, params string[] tags)
        {
            XmlDocument xDoc = null;
            foreach (var tag in tags)
            {
                var products = doc.GetElementsByTagName(tag);


                if (products.Count > 0)
                {
                    for (int i = 0; i < products.Count; i++)
                    {
                        var attribute = doc.CreateAttribute("json", "Array", "http://james.newtonking.com/projects/json");
                        attribute.InnerText = "true";
                        var node = products.Item(i) as XmlElement;
                        node.Attributes.Append(attribute);
                    }
                }
            }

            xDoc = doc;
            doc = null;
            return xDoc;
        }

        public TypeCollectionModel.Types GetTypes()
        {
            return TypeCollection.GetTypeCollection();
        }

        public async Task<string> GetSerializedTypesXml()
        {
            var typesJson = TypeCollection.Serialize();

            return typesJson;
        }

        public void ResetTypesCollection()
        {
            TypeCollection = null;
            InitializingJsonData = null;
            TypesXMLData = null;
        }
    }
}