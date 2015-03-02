using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WallExporter.Json
{

    [DataContract(Name = "post")]
    public class WallPost // : ISearchable
    {
        [DataMember(Name = "id")]
        public long id { get; set; }

        [DataMember(Name = "owner_id")]
        public long owner_id { get; set; }

        [DataMember(Name = "from_id")]
        public long from_id { get; set; }

        [DataMember(Name = "date")]
        public long date { get; set; }

        [DataMember(Name = "text")]
        public string text { get; set; }

        [DataMember(Name = "reply_owner_id")]
        public long reply_owner_id { get; set; }

        [DataMember(Name = "reply_post_id")]
        public long reply_post_id { get; set; }

        [DataMember(Name = "friends_only")]
        public int friends_only { get; set; }

        [DataMember(Name = "comments")]
        public VKComments comments { get; set; }

        [DataMember(Name = "likes")]
        public VKLikes likes { get; set; }

        [DataMember(Name = "reposts")]
        public VKReposts reposts { get; set; }

        [DataMember(Name = "post_type")]
        public string post_type { get; set; }

        [DataMember(Name = "post_source")]
        public VKPostSource post_source { get; set; }

        [DataMember(Name = "attachments")]
        public List<VKAttachment> attachments { get; set; }

        [DataMember(Name = "geo")]
        public VKGeo geo { get; set; }

        [DataMember(Name = "signer_id")]
        public long signer_id { get; set; }

        [DataMember(Name = "copy_history")]
        public List<WallPost> copy_history { get; set; }

        [DataMember(Name = "can_pin")]
        public int can_pin { get; set; }

        [DataMember(Name = "is_pinned")]
        public int is_pinned { get; set; }
    }


    public class VKComments
    {
        [DataMember(Name = "count")]
        public int count { get; set; }

        [DataMember(Name = "can_post")]
        public int can_post { get; set; }

    }

    public class VKLikes
    {
        [DataMember(Name = "count")]
        public int count { get; set; }
        [DataMember(Name = "user_likes")]
        public int user_likes { get; set; }
        [DataMember(Name = "can_like")]
        public int can_like { get; set; }
        [DataMember(Name = "can_publish")]
        public int can_publish { get; set; }
    }

    public class VKReposts
    {
        [DataMember(Name = "count")]
        public int count { get; set; }
        [DataMember(Name = "user_reposted")]
        public int user_reposted { get; set; }
    }

    public class VKPostSource
    {
        [DataMember(Name = "platform")]
        public string platform { get; set; }
        [DataMember(Name = "type")]
        public string type { get; set; }
        [DataMember(Name = "data")]
        public string data { get; set; }
    }
}
