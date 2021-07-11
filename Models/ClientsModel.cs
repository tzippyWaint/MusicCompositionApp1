using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ClientsModel
    {
        public ClientsModel()
        {
        }

        public int codeCli { get; set; }
        public Nullable<int> idC { get; set; }
        public string fullNameC { get; set; }
        public string pel1 { get; set; }
        public string pel2 { get; set; }
        public string email { get; set; }
        public Nullable<int> points { get; set; }
        public string status { get; set; }
    }
}
