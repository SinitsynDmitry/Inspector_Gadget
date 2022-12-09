using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspector_Gadget_Maui
{

    public class PhraseObj
    {
        public PhraseObj(string line,int id=0)
        {
            RawLine = line;
            var start= line.IndexOf('[');
            var stop = line.IndexOf(']');
            if (start >= 0 && stop > 0 && stop > start)
            {
                try
                {
                    var timeDiff = line.Substring(start + 1, stop - start - 1);
                    Text = line.Substring(stop + 1).Trim();
                    stop = line.IndexOf('-');
                    string[] format = new string[2] { @"h\:mm\:ss\.fff", @"mm\:ss\.fff" };

                    var fistTimeSpan = line.Substring(start + 1, stop - start - 1).Trim();

                    StartPosition = TimeSpan.ParseExact(fistTimeSpan, format, null, TimeSpanStyles.None);

                    start = stop + 3;

                    var lastTimeSpan = timeDiff.Substring(start).Trim();

                    StopPosition = TimeSpan.ParseExact(lastTimeSpan, format, null, TimeSpanStyles.None);
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                Text = line;
            }
        }

        public TimeSpan StartPosition
        {
            get; set;
        }

        public TimeSpan StopPosition
        {
            get; set;
        }

        public string Text
        {
            get; set;
        }
        public bool HighLight { get; set; }
        public string RawLine { get; set; }

        public int ID { get; set; }
    }
}
