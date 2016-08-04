using System.IO;
using System.Xml.Serialization;

namespace BlueprintEditor.Utilities
{
    public static class XmlUtilities
    {
        public static void SerializeToFile<T>(T obj, string fileName) where T : new()
        {
            var serializer = new XmlSerializer(typeof(T));

            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, obj);
                writer.Close();
            }
        }
        public static T DeserializeFromFile<T>(string fileName)
        {
            var serializer = new XmlSerializer(typeof(T));

            using (var reader = new StreamReader(fileName))
            {
                var obj = (T)serializer.Deserialize(reader);
                reader.Close();
                return obj;
            }
        }
    }
}