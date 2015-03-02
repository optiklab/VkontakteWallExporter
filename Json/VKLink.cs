using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallExporter.Json
{
    public class VKLink
    {
        public string url { get; set; }
        private string _title = "";
        public string title
        {
            get { return _title; }
            set
            {
                _title = (value ?? "");
            }
        }

        private string _desc = "";
        public string description
        {
            get { return _desc; }
            set
            {
                _desc = (value ?? "");
            }
        }
        public string image_src { get; set; }
    }
}
