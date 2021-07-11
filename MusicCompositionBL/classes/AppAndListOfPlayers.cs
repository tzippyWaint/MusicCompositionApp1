using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace MusicCompositionBL.classes
{
    public class AppAndListOfPlayers
    {
        //עצם בשביל קבלה של נתונים בקונטרולר כי אי אפשר לקבל בפןסט שני פרמטרים
        //להוספת הופעה
        public Appearances app { get; set; }
        public string dateA { get; set; }
        public string startHour { get; set; }
        public string endHour { get; set; }
        public Dictionary<int,int> listP { get; set; }
        public AppAndListOfPlayers()
        { }
    }
}
