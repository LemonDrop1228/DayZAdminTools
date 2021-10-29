using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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

            public static int DeepCount(this IEnumerable<IEnumerable<object>> collection)
            {
                var count = 0;
                foreach (var IenumCol in collection)
                {
                    count += IenumCol.Count();
                }
                return count;
            }

        #endregion

        #region String Extensions

        public static bool In<T>(this object value, params T[] comparisonArray)
        {
            return comparisonArray.Contains((T)value);
        }

        public static bool NullOrEmpty(this string value) => string.IsNullOrEmpty(value);

        #endregion

        #region LinqExtensions

        public static IEnumerable<TSource> LocalDistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new();
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
            xml.FormatXml( new() { Indent = indent, NewLineOnAttributes = newLineOnAttributes, IndentChars = indentChars, ConformanceLevel = conformanceLevel });

        public static string FormatXml(this string xml, XmlWriterSettings settings)
        {
            using (StringReader textReader = new(xml))
            using (XmlReader xmlReader = XmlReader.Create(textReader, new() { ConformanceLevel = settings.ConformanceLevel } ))
            using (StringWriter textWriter = new())
            {
                using (var xmlWriter = XmlWriter.Create(textWriter, settings))
                    xmlWriter.WriteNode(xmlReader, true);
                return textWriter.ToString();
            }
        }

        #endregion

        public static List<Type> GetTypesAssignableFrom<T1, T2>(this Assembly assembly)
        {
            return assembly.GetTypesAssignableFrom(typeof(T1), typeof(T2));
        }
        public static List<Type> GetTypesAssignableFrom(this Assembly assembly, Type compareType, Type excludeType)
        {
            List<Type> ret = new List<Type>();
            foreach (var type in assembly.DefinedTypes)
            {
                if (compareType.IsAssignableFrom(type) && compareType != type && excludeType != type)
                {
                    ret.Add(type);
                }
            }
            return ret;
        }
    }
}