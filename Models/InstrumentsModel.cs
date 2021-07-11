using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InstrumentsModel
    {
        public InstrumentsModel()
        {
        }

        public int codeInst { get; set; }
        public string nameInst { get; set; }
        public int style { get; set; }
        public int size { get; set; }
        public int voice { get; set; }
        public int type { get; set; }
    }
}
