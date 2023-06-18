using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PR4
{
    public class SerializationAndDeserialization
    {
        private static readonly string Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        public static T Serialization<T>(string fileName)
        {
            var filePath = System.IO.Path.Combine(Path, fileName);
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static void Deserialization<T>(T entries, string fileName)
        {
            var filePath = System.IO.Path.Combine(Path, fileName);
            var json = JsonConvert.SerializeObject(entries);
            File.WriteAllText(filePath, json);
        }
    }
}
