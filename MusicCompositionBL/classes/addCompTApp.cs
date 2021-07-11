using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCompositionBL.classes
{
   public class addCompTApp
    {
        //עצם בשביל קבלה של נתונים בקונטרולר כי אי אפשר לקבל בפןסט שני פרמטרים
        //להוספת הרכב
        public string style { get; set; }
        public int numOfP { get; set; }
        public List<Models.Instruments> listOfInst { get; set; }
        public addCompTApp()
        { }
    }
}
