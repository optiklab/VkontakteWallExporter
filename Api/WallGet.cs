using System;
using System.Net;
using System.Collections.Generic;
using WallExporter.Json;
using System.Diagnostics;
using WallExporter.Infrastructure;
using System.IO;

namespace WallExporter.Api
{
    public class WallGet : APIRequest
    {
        public WallGet(string owner_id, string domain, int offset, int count, string filter, Action<IList<WallPost>> callback)
            : base("wall.get")
        {
            base.AddParameter("owner_id", owner_id);
            base.AddParameter("domain", domain);
            base.AddParameter("offset", offset.ToString());
            base.AddParameter("count", count.ToString());
            base.AddParameter("filter", filter);
            base.AddParameter("extended", "1");

            base.SetSuccessHandler(_ParseResponse);

            _callback = callback;
        }

        private void _ParseResponse(string response)
        {
            try
            {
                response = SerializeHelper.FixResponseArrayString(response);

                var wall = SerializeHelper.Deserialise<WallResponse>(response);

                if (wall.WallPostResponse.WallPosts != null)
                {
                    _callback(wall.WallPostResponse.WallPosts);
                }
            }
            catch
            {
                Debug.WriteLine("Parse response from WallGet failed.");

                _callback(new List<WallPost>());
            }
        }

        private Action<IList<WallPost>> _callback;
    }
}
