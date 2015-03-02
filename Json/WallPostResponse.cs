using System.Collections.Generic;
using System.Runtime.Serialization;
using WallExporter.Json;

namespace WallExporter.Json
{
    //[DataContract]
    //public class WallPostResponse
    //{
    //    #region Public properties

    //    [DataMember(Name = "response")]
    //    public IList<WallPost> WallPosts { get; set; }

    //    #endregion
    //}

    [DataContract]
    public class WallPostResponse
    {
        #region Public properties

        [DataMember(Name = "wall")]
        public IList<WallPost> WallPosts { get; set; }

        [DataMember(Name = "groups")]
        public IList<VKGroup> Groups { get; set; }

        [DataMember(Name = "profiles")]
        public IList<VKUser> Profiles { get; set; }

        #endregion
    }
}
