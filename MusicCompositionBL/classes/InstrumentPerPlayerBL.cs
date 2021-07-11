using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCompositionDAL;

namespace MusicCompositionBL.classes
{
    class InstrumentPerPlayerBL
    {
        DBConection dbCon;
        List<InstumentPerPlayer> listOfInstrumentsPerPlayer;
        public InstrumentPerPlayerBL()
        {
            dbCon = new DBConection();
            listOfInstrumentsPerPlayer = dbCon.GetDbSet<InstumentPerPlayer>().ToList();
        }
        //הצגת כלים של נגן
        public List<InstumentPerPlayer> GetAllInstrumentsPerPlayers()
        {
            return listOfInstrumentsPerPlayer;
        }
    }
}
