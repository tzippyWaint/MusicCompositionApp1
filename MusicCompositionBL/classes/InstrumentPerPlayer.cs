using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCompositionDAL;
using Models;
namespace MusicCompositionBL.classes
{
    public class InstrumentPerPlayer
    {
        DBConection dbCon;
        public List<Models.InstumentPerPlayer> listOfInstrumentsPerPlayer;
        public InstrumentPerPlayer()
        {
            dbCon = new DBConection();
            listOfInstrumentsPerPlayer = dbCon.GetDbSet<InstumentPerPlayer>().ToList();
        }
        //הצגת כלים של נגן
        public List<InstumentPerPlayer> GetAllInstrumentsPerPlayers()
        {
            return listOfInstrumentsPerPlayer;
        }
        //הוספת כלי לנגן
        public string InsertInstrumPerPlayer(Models.InstumentPerPlayer inst)
        {
            try
            {
                dbCon.Execute<InstumentPerPlayer>(new InstumentPerPlayer() { codeIPerP = listOfInstrumentsPerPlayer.Max(i => i.codeIPerP) + 1, codeP = inst.codeP, priceOfAppearance = inst.priceOfAppearance, rating = inst.rating ,codeInst=inst.codeInst}, DBConection.ExecuteActions.Insert);
                listOfInstrumentsPerPlayer = dbCon.GetDbSet<InstumentPerPlayer>().ToList();
                return "true";
            }
            catch
            {
                return "false";
            }
        }
        //מחיקת כלי פר נגן
        public bool DeleteInstrumPerPlayer(int codeIPerP)
        {
            try
            {
                dbCon.Execute<InstumentPerPlayer>(listOfInstrumentsPerPlayer.First(p => p.codeIPerP == codeIPerP), DBConection.ExecuteActions.Delete);
                listOfInstrumentsPerPlayer = dbCon.GetDbSet<InstumentPerPlayer>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //עדכון כלי של נגן
        public bool UpDateInstrumPerPlayer(InstumentPerPlayer instumentPerPlayer)
        {
            try
            {
                dbCon.Execute<InstumentPerPlayer>(instumentPerPlayer, DBConection.ExecuteActions.Update);
                listOfInstrumentsPerPlayer = dbCon.GetDbSet<InstumentPerPlayer>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Models.InstumentPerPlayer ConvertInstumentPerPlayerToEF(InstrumentPerPlayerModel i)
        {
            return new Models.InstumentPerPlayer
            {
                codeIPerP = i.codeIPerP,
                codeInst = i.codeInst,
                codeP = i.codeP,
                priceOfAppearance = i.priceOfAppearance,
                rating = i.rating
            };
        }
        public InstrumentPerPlayerModel ConvertInstumentPerPlayerToModel(Models.InstumentPerPlayer i)
        {
            return new InstrumentPerPlayerModel
            {
                codeIPerP=i.codeIPerP,codeInst=i.codeInst,codeP=i.codeP,priceOfAppearance=i.priceOfAppearance,rating=i.rating

            };
        }
        public List<InstrumentPerPlayerModel> ConvertListToModel(List<Models.InstumentPerPlayer> i)
        {
            List<InstrumentPerPlayerModel> l = new List<InstrumentPerPlayerModel>();
            Models.InstumentPerPlayer inst = new Models.InstumentPerPlayer();
            foreach (var item in i)
            {
                inst.codeIPerP = item.codeIPerP;
                inst.codeInst = item.codeInst;
                inst.codeP = item.codeP;
                inst.priceOfAppearance = item.priceOfAppearance;
                inst.rating = item.rating;
                l.Add(ConvertInstumentPerPlayerToModel(inst));
            }
            return l;
        }

    }
}
