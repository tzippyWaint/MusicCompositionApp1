using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace MusicCompositionBL.classes
{
  public  class insertPlayer
    {
        //משמשת לשליחת פרמטרים לפעולת הוספת נגן
        public insertPlayer()
        { }
        public Players player { get; set; }
        public List<MusicCompositionBL.classes.DetailsOfInstrumOfPlayer> inst { get; set; }
    }
}
