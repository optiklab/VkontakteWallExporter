using System.Collections.Generic;
using System.Runtime.Serialization;
using WallExporter.Json;

namespace WallExporter.Json
{
    [DataContract]
    public class VKList<T>
    {
        public int count { get; set; }

        public List<T> items { get; set; }
    }
}
