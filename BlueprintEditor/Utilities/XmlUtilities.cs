using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace BlueprintEditor.Extensions
{
    public static class XmlUtilities
    {
        public static void SerializeToFile<T>(T obj, string fileName) where T : new()
        {
            if (!typeof(T).IsSerializable && !(typeof(ISerializable).IsAssignableFrom(typeof(T))))
                throw new InvalidOperationException("Object must be serializable.");

            var serializer = new XmlSerializer(typeof(T));

            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, obj);
                writer.Close();
            }
        }

        public static void DeserializeFromFile<T>(string fileName, out T obj)
        {
            if (!typeof(T).IsSerializable && !(typeof(ISerializable).IsAssignableFrom(typeof(T))))
                throw new InvalidOperationException("Object must be serializable.");

            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(fileName))
            {
                obj = (T)serializer.Deserialize(reader);
                reader.Close();
            }
        }
    }
}
