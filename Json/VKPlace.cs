using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallExporter.Json
{
    public class Place
    {
        private string _title = "";
        public string title
        {
            get { return _title; }
            set
            {
                _title = (value ?? "");
            }
        }

        private string _address = "";
        public string address
        {
            get { return _address; }
            set
            {
                _address = (value ?? "");
            }
        }

        public double latitude { get; set; }

        public double longitude { get; set; }
     
        public string country { get; set; }

        public string city { get; set; }

        public string icon { get; set; }

        public string type { get; set; }

        public long group_id { get; set; }

        public string group_photo { get; set; }

        public int checkins { get; set; }

        public long updated { get; set; }

    }
}
