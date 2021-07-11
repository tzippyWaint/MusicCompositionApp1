using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InstrumentPerPlayerModel
    {
        public int codeIPerP { get; set; }
        public int codeP { get; set; }
        public int codeInst { get; set; }
        public Nullable<int> priceOfAppearance { get; set; }
        public Nullable<int> rating { get; set; }
    }
}
