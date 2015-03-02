using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WallExporter.Json
{
    public class VKPoll
    {
        public long id { get; set; }

        public long owner_id { get; set; }

        public long created { get; set; }

        public int is_closed { get; set; }

        private string _question = "";
        public string question
        {
            get
            {
                return _question;
            }
            set
            {
                _question = (value ?? "");
            }
        }

        public int votes { get; set; }

        public long answer_id { get; set; }

        public List<VKPollAnswer> answers { get; set; }
    }

    public class VKPollAnswer
    {
        public long id { get; set; }

        private string _text = "";
        public string text { get { return _text; } set { _text = (value ?? ""); } }
        public int votes { get; set; }
        public double rate { get; set; }
    }
}
