﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WallExporter.Json
{
    public class VKNote
    {
        public long id { get; set; }

        public long user_id { get; set; }
        public long owner_id { get { return user_id; } set { user_id = value; } }

        private string _title = "";
        public string title
        {
            get { return _title; }
            set
            {
                _title = (value ?? "");
            }
        }


        private string _text = "";
        public string text
        {
            get { return _text; }
            set
            {
                _text = (value ?? "");
            }
        }


        public int comments { get; set; }

        public int read_comments { get; set; }

        public string view_url { get; set; }
    }
}
