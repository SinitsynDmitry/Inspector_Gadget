using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inspector_Gadget_Maui
{

    public class PhraseObj
    {

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
    }
}
