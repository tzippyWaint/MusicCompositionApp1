using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCompositionDAL;
using Models;
namespace MusicCompositionBL.classes
{
    public class PlayerInAppearenceBL
    {
        DBConection dbCon;
        public List<PlayersInAppearances> listOfPlayersInAppearances;
        public PlayerInAppearenceBL()
        {
            dbCon = new DBConection();
            listOfPlayersInAppearances = dbCon.GetDbSet<PlayersInAppearances>().ToList();
        }
        public List<PlayersInAppearances> GetAllPlayersInAppearances()
        {
            return listOfPlayersInAppearances;
        }
        //הוספת נגן בהופעה
        public bool InsertPlayerInApp(int codeA, int codeP, int codeInst)
        {
            try
            {
                dbCon.Execute<PlayersInAppearances>(new PlayersInAppearances() { codePInA = listOfPlayersInAppearances.Max(i => i.codePInA) + 1, codeA = codeA, codeP = codeP ,codeInst=codeInst}, DBConection.ExecuteActions.Insert);
                listOfPlayersInAppearances = dbCon.GetDbSet<PlayersInAppearances>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //מחיקת נגן בהופעה
        public bool DeletePlayerInApp(int codePinA)
        {
            try
            {
                dbCon.Execute<PlayersInAppearances>(listOfPlayersInAppearances.First(p => p.codePInA == codePinA), DBConection.ExecuteActions.Delete);
                listOfPlayersInAppearances = dbCon.GetDbSet<PlayersInAppearances>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<Players> getPlayersInAppByCodeApp(int codeA)
        {
            PlayersBL pBl = new PlayersBL();
            InstrumentsBL i = new InstrumentsBL();
            List<Players> list = new List<Players>();
            foreach (var item in listOfPlayersInAppearances)
            {
                if (item.codeA == codeA)
                {
                    list.Add(pBl.listOfAllPlayers.Find(p => p.codeP == item.codeP));
                    list[list.Count-1].status = i.listOfInstruments.Find(o => o.codeInst == item.codeInst).nameInst;
                }
            }
            return list;
        }
        public PlayersInAppearances ConvertPlayersInAppearancesToEF(PlayerInApperanceModel i)
        {
            return new PlayersInAppearances
            {
                codeA = i.codeA,
                codeP = i.codeP,
                codePInA = i.codePInA,
                codeInst=i.codeInst
            };
        }
        public static PlayerInApperanceModel ConvertPlayersInAppearancesToModel(PlayersInAppearances i)
        {
            return new PlayerInApperanceModel
            {
                codeA = i.codeA,
                codeP = i.codeP,
                codePInA = i.codePInA,
                codeInst = i.codeInst
            };
        }

    }
}
