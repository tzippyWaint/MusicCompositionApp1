using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCompositionDAL;
using Models;
namespace MusicCompositionBL.classes
{
    public class AppearancesBL
    {
        public DBConection dbCon;
        public List<Appearances> listOfAppearances;
        CompositionBL CompositionBL = new CompositionBL();
        PlayerInAppearenceBL playerInAppearenceBL = new PlayerInAppearenceBL();
        public AppearancesBL()
        {
            dbCon = new DBConection();
            listOfAppearances = dbCon.GetDbSet<Appearances>().ToList();
        }
        public List<Appearances> GetAllApearances()
        {
            return listOfAppearances;
        }
        public List<AppearancesModel> GetAllApearancesModels()
        {
            return ConvertListToModel(listOfAppearances);
        }
        //הוספת הופעה
        public bool InsertAppearance(Appearances appearance, Dictionary<int,int> dictPlayersandinst,string date,string start,string end)
        {
            List<string> ListForEmail=new List<string> ();
            List<Players> listPlayersInComp = GetPlayersByDict(dictPlayersandinst);
                try
                {
                TimeSpan startt = TimeSpan.Parse(start);
                DateTime datee = DateTime.Parse(date);
                TimeSpan endd = TimeSpan.Parse(end);
                int codeC=190;
                if(listPlayersInComp.Find(p => p.status == "activeC") != null)
                    codeC= listPlayersInComp.Find(p => p.status == "activeC").codeP;
                    dbCon.Execute<Appearances>(new Appearances() { dateA = datee, addresPlace = appearance.addresPlace, codeCli = appearance.codeCli, pelPlays = appearance.pelPlays, startHour = startt, endHour = endd, codeComp = CompositionBL.listOfCompositions.Max(c => c.codeComp), codeConductor = codeC, cost = appearance.cost }, DBConection.ExecuteActions.Insert);
                    listOfAppearances = dbCon.GetDbSet<Appearances>().ToList();
                    foreach (var item in listPlayersInComp)
                    {
                        playerInAppearenceBL.InsertPlayerInApp(listOfAppearances.Max(a => a.codeA), item.codeP,dictPlayersandinst[(int)item.idP]);
                        ListForEmail.Add(item.email);         
                    }
                    //פעולת שליחה של אימייל לכל נגן
                    MusicCompositionBL.EmailAndPayPal.SendMailToPlayers(listOfAppearances.Find(a => a.codeA == listOfAppearances.Max(aa => aa.codeA)), ListForEmail,"הופעה חדשה בתאריך",0);
                    return true;
                }
                catch
                {
                    return false;
                }
        }

        private List<Players> GetPlayersByDict(Dictionary<int, int> dictPlayersandinst)
        {
            List<Players> list = new List<Players>();
            PlayersBL playersBL = new PlayersBL();
            foreach (var item in dictPlayersandinst)
            {
                list.Add(playersBL.listOfPlayers.Find(p => p.idP == item.Key));
            }
            return list;
        }

        //מחיקת הופעה
        public int DeleteAppearance(Appearances appearance)
        {
            try
            {
                MusicCompositionBL.classes.PlayersBL playersBL = new PlayersBL();
                List<string> accountments = new List<string>();
                if (DateTime.Parse(listOfAppearances.First(a => a.codeA == appearance.codeA).dateA.ToString()).Day >= DateTime.Now.Day + 14)
                {
                    //מחיקת הנגנים מהופעה
                    foreach (var item in playerInAppearenceBL.listOfPlayersInAppearances)
                    {
                        //שלחית הודעת אימייל אודות ביטול ההופעה
                        if (item.codeA == appearance.codeA)
                            playerInAppearenceBL.DeletePlayerInApp(item.codePInA);
                        accountments.Add(playersBL.listOfPlayers.Find(p => p.codeP == item.codeP).email);
                    }
                    MusicCompositionBL.EmailAndPayPal.SendMailToPlayers(appearance,accountments , "ביטול הופעה בתאריך", 1);
                    //מחיקת ההרכב של ההופעה
                    CompositionBL.DeleteComposition(CompositionBL.listOfCompositions.Find(c=>c.codeComp== listOfAppearances.First(a => a.codeA == appearance.codeA).codeComp));
                    dbCon.Execute<Appearances>(listOfAppearances.First(c => c.codeA == appearance.codeA), DBConection.ExecuteActions.Delete);
                    listOfAppearances = dbCon.GetDbSet<Appearances>().ToList();
                    return 1;
                }
               else
                   return 2;
            }
            catch
            {
                return 0;
            }
        }
        //עדכון הופעה
        public int UpDateAppearance(Appearances appearanceToUpdate)
        {
            try
            {
                dbCon.Execute<Appearances>(appearanceToUpdate, DBConection.ExecuteActions.Update);
                listOfAppearances = dbCon.GetDbSet<Appearances>().ToList();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public Appearances ConvertAppearancesToEF(AppearancesModel app)
        {
            return new Appearances
            {
                codeA=app.codeA, codeCli=app.codeCli,codeComp=app.codeComp,codeConductor=app.codeConductor,cost=app.cost,addresPlace=app.addresPlace,dateA=app.dateA,startHour=app.startHour,endHour=app.endHour
            };
        }
        public static AppearancesModel ConvertAppearancesToModel(Appearances app)
        {
            return new AppearancesModel
            {
                codeA = app.codeA,
                codeCli = app.codeCli,
                codeComp = app.codeComp,
                codeConductor = app.codeConductor,
                cost = app.cost,
                addresPlace = app.addresPlace,
                dateA = app.dateA,
                startHour = app.startHour,
                endHour = app.endHour,
                pelPlays=app.pelPlays

            };
        }
        public  List<AppearancesModel> ConvertListToModel(List<Appearances> i)
        {
            List<AppearancesModel> l = new List<AppearancesModel>();
            Appearances app = new Appearances();
            foreach (var item in i)
            {
                app.codeA = item.codeA;
                app.codeCli = item.codeCli;
                app.codeComp = item.codeComp;
                app.codeConductor = item.codeConductor;
                app.cost = item.cost;
                app.addresPlace = item.addresPlace;
                app.dateA = item.dateA;
                app.startHour = item.startHour;
                app.endHour = item.endHour;
                app.pelPlays = item.pelPlays;
                l.Add(ConvertAppearancesToModel( app));
            }
            return l;
        }
        public static List<Appearances> ConvertListToEF(List<AppearancesModel> i)
        {
            List<Appearances> l = new List<Appearances>();
            Appearances app = new Appearances();
            foreach (var item in i)
            {
                app.codeA = item.codeA;
                app.codeCli = item.codeCli;
                app.codeComp = item.codeComp;
                app.codeConductor = item.codeConductor;
                app.cost = item.cost;
                app.addresPlace = item.addresPlace;
                app.dateA = item.dateA;
                app.startHour = item.startHour;
                app.endHour = item.endHour;
                app.pelPlays = item.pelPlays;
                l.Add(app);
            }
            return l;
        }
    }
}
