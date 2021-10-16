using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml;

namespace DayZTediratorToolz.Helpers
{
    public static class DayZTediratorToolzHelperExtensions
    {
        #region IEnumerable Extensions

            public static void AddRange<T>(this ObservableCollection<T> collection, params T[] items)
            {
                foreach (var item in items)
                {
                    if(!collection.Contains(item))
                        collection.Add(item);
                }
            }
            
        #endregion

        #region String Extensions

        public static bool In<T>(this object value, params T[] comparisonArray)
        {
            return comparisonArray.Contains((T)value);
        }

        #endregion

        #region LinqExtensions

        public static IEnumerable<TSource> LocalDistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        #endregion
        
        #region XmlExtensions

        public static string FormatXml(this string xml, bool indent = true, bool newLineOnAttributes = false, string indentChars = "  ", ConformanceLevel conformanceLevel = ConformanceLevel.Document) => 
            xml.FormatXml( new XmlWriterSettings { Indent = indent, NewLineOnAttributes = newLineOnAttributes, IndentChars = indentChars, ConformanceLevel = conformanceLevel });

        public static string FormatXml(this string xml, XmlWriterSettings settings)
        {
            using (var textReader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(textReader, new XmlReaderSettings { ConformanceLevel = settings.ConformanceLevel } ))
            using (var textWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                    xmlWriter.WriteNode(xmlReader, true);
                return textWriter.ToString();
            }
        }

        #endregion
    }
}