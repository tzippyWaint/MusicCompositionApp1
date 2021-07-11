using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class AppearancesModel
    {
        public AppearancesModel()
        {
        }

        public int codeA { get; set; }
        public Nullable<System.DateTime> dateA { get; set; }
        public string addresPlace { get; set; }
        public int codeCli { get; set; }
        public string pelPlays { get; set; }
        public Nullable<System.TimeSpan> startHour { get; set; }
        public Nullable<System.TimeSpan> endHour { get; set; }
        public int codeComp { get; set; }
        public int codeConductor { get; set; }
        public Nullable<int> cost { get; set; }
    }
}
