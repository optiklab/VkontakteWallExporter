using System.Collections.Generic;
using System.Runtime.Serialization;
using WallExporter.Json;

namespace WallExporter.Json
{
    [DataContract]
    public class WallResponse
    {
        [DataMember(Name = "response")]
        public WallPostResponse WallPostResponse { get; set; }
    }
}
