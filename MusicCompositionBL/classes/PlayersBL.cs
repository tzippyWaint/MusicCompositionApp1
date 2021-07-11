using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCompositionDAL;
using Models;
namespace MusicCompositionBL.classes
{
    public class PlayersBL
    {
        DBConection dbCon;
        public List<Players> listOfPlayers;

        public List<Players> listOfAllPlayers;
        List<Players> groupFromAvailablePlayers = new List<Players>();
        PlayerInAppearenceBL playerInAppearenceBL = new PlayerInAppearenceBL();
        MusicCompositionBL.classes. InstrumentPerPlayer instrumentPerPlayer = new MusicCompositionBL.classes.InstrumentPerPlayer();
        int[,] costs;
        public PlayersBL()
        {
            dbCon = new DBConection();
            listOfAllPlayers = dbCon.GetDbSet<Players>().ToList();
            listOfPlayers = (dbCon.GetDbSet<Players>().ToList()).Where(p=>p.status!="manager").ToList();
        }
        
        public List<Players> GetAllPlayers()
        {
            return listOfAllPlayers;
        }
        //הוספת נגן
        public string InsertPlayer(classes.insertPlayer add)
        {
                if (listOfPlayers.Find(p => p.idP == add.player.idP) == null)
                    try
                    {

                        dbCon.Execute<Players>(new Players() { codeP = listOfPlayers.Max(c => c.codeP) + 1, idP = add.player.idP, fullNameP = add.player.fullNameP, pel = add.player.pel, email = add.player.email, daysWork = add.player.daysWork, status = add.player.status }, DBConection.ExecuteActions.Insert);
                        listOfPlayers = dbCon.GetDbSet<Players>().ToList();
                        foreach (var i in add.inst)
                        {
                            instrumentPerPlayer.InsertInstrumPerPlayer(new Models.InstumentPerPlayer() { codeInst=i.instrument.codeInst,codeP=add.player.codeP,priceOfAppearance=i.price,rating=0});
                        }
                        return "true";
                    }
                    catch
                    {
                        return "false";
                    }
                else
                {
                    return "existing player with same id";
                }
          
        }
        //מחיקת נגן
        public int DeletePlayer(Players player)
        {
            try
            {
                if (listOfPlayers.First(p => p.idP == player.idP) != null)
                {
                    if (playerInAppearenceBL.listOfPlayersInAppearances.Where(p => p.codeP == player.codeP) != null)
                    {
                    //מחיקת הכלים שלו
                    foreach (var item in instrumentPerPlayer.listOfInstrumentsPerPlayer)
                    {
                        if(item. codeP== player.codeP)
                            instrumentPerPlayer.DeleteInstrumPerPlayer(item.codeIPerP);
                    }
                    dbCon.Execute<Players>(listOfPlayers.First(p => p.idP == player.idP), DBConection.ExecuteActions.Delete);
                    listOfPlayers = dbCon.GetDbSet<Players>().ToList();
                        return 1;
                    }
                }
                return 2;
            }
            catch
            {
                return 0;
            }
        }
        //עדכון נגן
        public bool UpDatePlayer(Players player)
        {
            try
            {
                dbCon.Execute<Players>(player, DBConection.ExecuteActions.Update);
                listOfAllPlayers = dbCon.GetDbSet<Players>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //בחירת נגנים להרכב מסויים על פי כלי , זמינות, רייטינג ומחיר
        public Dictionary<int?, int> PlayerForAppearance(int codeComp,DateTime dateAppearance)
        {
            //דגל האם  הנגן תפוס באותו תאריך
            bool reserved = false; 
            //רשימת הנגנים הפנויים
            var listOfAvailablePlayers=new List<Players>();        
            foreach (var player in listOfPlayers)
            {
                //האם הנגן עובד ביום הזה בשבוע
                if(WorkDayesOfPlayer((dateAppearance.DayOfWeek.ToString()),player.daysWork))
                { 
                  foreach (var item in playerInAppearenceBL.listOfPlayersInAppearances)
                  {
                      if (item.codeP == player.codeP && new AppearancesBL().listOfAppearances.First(a => a.codeA == item.codeA).dateA == dateAppearance)
                      {
                          reserved = true;
                          break;
                      }    
                  }
                  if (!reserved)
                      listOfAvailablePlayers.Add(player);
                } 
            }
            //רשימת הכלים בהרכב
            var listOfInstrumentsForCopm = new List<InstrumentsModel>();
            foreach (var item in new InstrumOfCompBL().listOfInstrumInCompositions)
            {
                if (item.codeComp == codeComp)
                    listOfInstrumentsForCopm.Add(new InstrumentsBL().listOfInstruments.First(i => i.codeInst == item.codeInst));
            }
            //listOfInstrumentsForCopm.Add(new InstrumentsBL().listOfInstruments.First(i => i.codeInst == 49));
            List<Models.InstumentPerPlayer> listOfInstrumentsPerPlayer = new List<Models.InstumentPerPlayer>();
            var instPerPBL=new classes.InstrumentPerPlayer();
            Models.InstumentPerPlayer nowinst = new Models.InstumentPerPlayer();
            //var groupFromAvailablePlayers = listOfPlayers;
            int[] agentsTasks;
            int? minCost = int.MaxValue;
            Dictionary<int?, int> settingPlayerPerInstrum = new Dictionary<int?, int>();
            Dictionary<int?, int> settingPlayerPerInstrumEnding = new Dictionary<int?, int>();
            int? costPerOption = 0;
            for (int i=0;i< (listOfAvailablePlayers.Count/ listOfInstrumentsForCopm.Count);i++)
            {
                agentsTasks=FillCosts(listOfInstrumentsForCopm,listOfAvailablePlayers,i,1,dateAppearance);
                costPerOption = 0;
                settingPlayerPerInstrum.Clear();
                for (int s = 0; s < agentsTasks.Count(); s++)
                   {                      
                        settingPlayerPerInstrum.Add(groupFromAvailablePlayers[s].idP, listOfInstrumentsForCopm[agentsTasks[s]].codeInst);
                   }
                costPerOption = GetCostOfApp(settingPlayerPerInstrum);
                if (costPerOption < minCost)
                {
                   minCost = costPerOption;
                   settingPlayerPerInstrumEnding = settingPlayerPerInstrum;
                }
                groupFromAvailablePlayers.Clear();  
            }
            //מוטציות
            for (int i = 0; i < (listOfAvailablePlayers.Count / listOfInstrumentsForCopm.Count)-1; i++)
            {
                agentsTasks = FillCosts(listOfInstrumentsForCopm, listOfAvailablePlayers, i, 2,dateAppearance);
                costPerOption = 0;

                settingPlayerPerInstrum.Clear();
                for (int s = 0; s < agentsTasks.Count(); s++)
                {
                    settingPlayerPerInstrum.Add(groupFromAvailablePlayers[s].idP, listOfInstrumentsForCopm[agentsTasks[s]].codeInst);
                }
                costPerOption = GetCostOfApp(settingPlayerPerInstrum);
                if (costPerOption < minCost)
                {
                    minCost = costPerOption;
                    settingPlayerPerInstrumEnding = settingPlayerPerInstrum;
                }
                groupFromAvailablePlayers.Clear();
            }
            //חלוקה עם שארית
            if((listOfAvailablePlayers.Count % listOfInstrumentsForCopm.Count)!= 0)
            {
                agentsTasks = FillCosts(listOfInstrumentsForCopm, listOfAvailablePlayers, 0, 1, dateAppearance);
                costPerOption = 0;
                //for (int f = 0; f < agentsTasks.Count(); f++)
                //{
                //    costPerOption += costs[f, agentsTasks[f]];
                //}

                settingPlayerPerInstrum.Clear();
                for (int s = 0; s < agentsTasks.Count(); s++)
                {
                    settingPlayerPerInstrum.Add(groupFromAvailablePlayers[s].idP, listOfInstrumentsForCopm[agentsTasks[s]].codeInst);
                }
                costPerOption = GetCostOfApp(settingPlayerPerInstrum);
                if (costPerOption < minCost)
                {
                    minCost = costPerOption;
                    settingPlayerPerInstrumEnding = settingPlayerPerInstrum;
                }
            } 
            return settingPlayerPerInstrumEnding;
        }
        //מילוי מטריצת עלויות
        private int[] FillCosts(List<InstrumentsModel> listOfInstrumentsForCopm, List<Players> listOfAvailablePlayers,int i,int motations,DateTime dateAppearance)
        {
            costs = new int[listOfInstrumentsForCopm.Count, listOfInstrumentsForCopm.Count];
            int costPerPlayerAndInstrum = 0;
            List<Models.InstumentPerPlayer> listOfInstrumentsPerPlayer = new List<Models.InstumentPerPlayer>();
            var instPerPBL = new classes.InstrumentPerPlayer();
            bool degel = false;
            Models.InstumentPerPlayer nowinst = new Models.InstumentPerPlayer();
           
            int[] agentsTasks;
            //קבוצה מתוך הנגנים כמספר הכלים
            if(motations==2)
            for (int j = 0; j < listOfInstrumentsForCopm.Count*2; j+=2)
            {
                groupFromAvailablePlayers.Add(listOfAvailablePlayers[j + (listOfInstrumentsForCopm.Count * i)]);
            }
            else
                if(motations==1)
                for (int j = 0; j < listOfInstrumentsForCopm.Count; j ++)
                {
                    groupFromAvailablePlayers.Add(listOfAvailablePlayers[j + (listOfInstrumentsForCopm.Count * i)]);
                }
            else
            {
                for (int k = listOfAvailablePlayers.Count % listOfInstrumentsForCopm.Count; k < listOfAvailablePlayers.Count; k++)
                {
                    groupFromAvailablePlayers.Add(listOfAvailablePlayers[k]);
                }
                for (int t = 0; t < listOfInstrumentsForCopm.Count - groupFromAvailablePlayers.Count; t++)
                {
                    groupFromAvailablePlayers.Add(listOfAvailablePlayers[t]);
                }
            }

            for (int player = 0; player < groupFromAvailablePlayers.Count; player++)
            {
                //רשימת הכלים שהנגן מנגן בהם
                listOfInstrumentsPerPlayer = instPerPBL.listOfInstrumentsPerPlayer.Where(inst => inst.codeP == groupFromAvailablePlayers[player].codeP).ToList();
                for (int inst = 0; inst < listOfInstrumentsForCopm.Count; inst++)
                {
                    degel = false;
                    costPerPlayerAndInstrum = 0;
                    //האם מנגן על הכלי
                    foreach (var item in listOfInstrumentsPerPlayer)
                    {
                        if (item.codeInst == listOfInstrumentsForCopm[inst].codeInst)
                        {
                            degel = true;
                            nowinst = item;
                            break;
                        }
                    }
                    if (!degel)
                    {
                        costs[player, inst] = int.MaxValue;
                        continue;
                    }
                    AppearancesBL appearancesBL = new AppearancesBL();
                    Appearances appearance = new Appearances();
                    //תוספת 10 נקודות על כל הופעה שיש לו החודש
                    foreach (var item in playerInAppearenceBL.listOfPlayersInAppearances)
                    {
                        appearance = appearancesBL.listOfAppearances.First(a => a.codeA == item.codeA);
                        if (item.codeP == listOfAvailablePlayers[player].codeP && appearance.dateA?.Month == dateAppearance.Month)
                            costPerPlayerAndInstrum += 10;
                    }
                    //רייטינג
                    if (nowinst.rating <= 10)
                        costPerPlayerAndInstrum += 10;
                    else
                        if (nowinst.rating <= 20)
                        costPerPlayerAndInstrum += 6;
                    else
                        if (nowinst.rating <= 40)
                        costPerPlayerAndInstrum += 4;
                    else
                        if (nowinst.rating <= 60)
                        costPerPlayerAndInstrum += 2;
                    else
                        if (nowinst.rating <= 100)
                        costPerPlayerAndInstrum += 1;
                    //מחיר עבור הכלי
                    costPerPlayerAndInstrum += int.Parse(nowinst.priceOfAppearance.ToString());
                    costs[player, inst] = costPerPlayerAndInstrum;
                 
                }
            }
            //מערך בו כל מקום מייצג את הכלי עבור הנגן
            return agentsTasks = HungarianAlgorithm.FindAssignments(costs);    
        }

        //האם הנגן עובד ביום הנתון
        private bool WorkDayesOfPlayer(string dayOfWeek, string daysWork)
        {
            int day = 0;
            switch(dayOfWeek)
            {
                case "Sunday":
                    day = 1;
                    break;
                case "Monday":
                    day = 2;
                    break;
                case "Tuesday":
                    day = 3;
                    break;
                case "Wednesday":
                    day = 4;
                    break;
                case "Thursday":
                    day = 5;
                    break;
            }

            if(daysWork[day-1]=='1')
              return true;
            return false;
        }

        //הצגת יומן אירועים לפי קוד נגן
        public List<AppearancesModel> ScheduleForPlayer(int codePlayer)
        {
            AppearancesBL appearancesBL = new AppearancesBL();
            InstrumentsBL instrumentsBL = new InstrumentsBL();
            List<PlayersInAppearances> appOfPlayer = playerInAppearenceBL.listOfPlayersInAppearances.Where(a => a.codeP == codePlayer).ToList();
            List<Appearances> appearances = new List<Appearances>();
            foreach (var item in appOfPlayer)
            {
                appearances.Add(appearancesBL.listOfAppearances.First(a => a.codeA == item.codeA));
                appearances[appearances.Count - 1].codeConductor =int.Parse( listOfPlayers.Find(p => p.codeP == appearances[appearances.Count - 1].codeConductor).pel);
                appearances[appearances.Count - 1].pelPlays =instrumentsBL.listOfInstruments.Find(i=>i.codeInst== item.codeInst).nameInst;
            }
            return appearancesBL.ConvertListToModel( appearances);
        }
        //עלות הופעה על פי רשימת השיבוץ
        public int? GetCostOfApp(Dictionary<int?, int> dict)
        {
           List<Models.InstumentPerPlayer> list= instrumentPerPlayer.listOfInstrumentsPerPlayer;
            int? cost = 0;
            foreach (var item in dict.Keys)
            {
                if (checkedPAndI(item, dict[item]))
                  cost += list.Find(i => (i.codeP == (listOfPlayers.FirstOrDefault(p => p.idP == item).codeP)) && (i.codeInst == dict[item])).priceOfAppearance;
            }
            return cost;
        }

        private bool checkedPAndI(int? item, int v)
        {
            int code = listOfPlayers.FirstOrDefault(p => p.idP == item).codeP;
            foreach (var i in instrumentPerPlayer.listOfInstrumentsPerPlayer)
            {
                if (i.codeP == code && i.codeInst == v)
                    return true;
            }
            return false;
        }
        public string GetNameByCodeP(int codeP)
        {
            return listOfAllPlayers.Find(p => p.codeP == codeP).fullNameP;
        }
        public Players ConvertPlayersToEF(PlayersModel i)
        {
            return new Players
            {
                codeP=i.codeP,idP=i.idP,fullNameP=i.fullNameP,pel=i.pel,email=i.email,daysWork=i.daysWork,status=i.status
            };
        }
        public static PlayersModel ConvertPlayersToModel(Players i)
        {
            return new PlayersModel
            {
                codeP = i.codeP,
                idP = i.idP,
                fullNameP = i.fullNameP,
                pel = i.pel,
                email = i.email,
                daysWork = i.daysWork,
                status = i.status
            };
        }
        public List<PlayersModel> ConvertListToModel(List<Players> i)
        {
            List<PlayersModel> l = new List<PlayersModel>();
            Players p = new Players();
            foreach (var item in i)
            {
                p.codeP = item.codeP;
                p.idP = item.idP;
                p.fullNameP = item.fullNameP;
                p.pel = item.pel;
                p.email = item.email;
                p.daysWork = item.daysWork;
                p.status = item.status;
                l.Add(ConvertPlayersToModel(p));
            }
            return l;
        }
    }
}
