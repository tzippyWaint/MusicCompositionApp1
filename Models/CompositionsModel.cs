using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CompositionsModel
    {
        public CompositionsModel(){
        }

        public int codeComp { get; set; }
        public string type { get; set; }
        public Nullable<int> numOfPlayers { get; set; }

    }
}
