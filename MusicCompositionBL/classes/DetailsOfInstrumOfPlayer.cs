using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCompositionDAL;
using Models;
namespace MusicCompositionBL.classes
{
    public class DetailsOfInstrumOfPlayer
    {
        //מיועדת לשימוש בהוספת נגן, כלי שמנגן בו והמחיר שלוקח להופעה בו
        public Instruments instrument { get; set; }
        public int price { get; set; }
        public DetailsOfInstrumOfPlayer()
        { }
    }
}
