using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class PlayersModel
    {
        public PlayersModel()
        {
        }

        public int codeP { get; set; }
        public Nullable<int> idP { get; set; }
        public string fullNameP { get; set; }
        public string pel { get; set; }
        public string email { get; set; }
        public string daysWork { get; set; }
        public string status { get; set; }
    }
}
