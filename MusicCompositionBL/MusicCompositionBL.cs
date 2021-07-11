using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MusicCompositionDAL;

namespace MusicCompositionBL
{
    public class MusicCompositionBL
    {
        #region data
        DBConection dbCon;
        Random random = new Random();
        int sizeOfPlace;
        //האם יש שדה קלט שהוזן לו ערך שגוי
        int itsOK = 0;
        //lists cf data
        List<Appearances> listOfAppearances;
        List<Clients> listOfClients;
        List<Instruments> listOfInstruments;
        List<Compositions> listOfCompositions;
        List<InstrumInCompositions> listOfInstrumInCompositions;
        List<Message> listOfMessage;
        List<Places> listOfPlaces;
        List<Players> listOfPlayers;
        List<PlayersInAppearances> listOfPlayersInAppearances;
        List<InstumentPerPlayer> listOfInstrumentsPerPlayer;
        #endregion
        public MusicCompositionBL()
        {
            dbCon = new DBConection();
            listOfAppearances = dbCon.GetDbSet<Appearances>().ToList();
            listOfClients = dbCon.GetDbSet<Clients>().ToList();
            listOfInstruments = dbCon.GetDbSet<Instruments>().ToList();
            listOfCompositions = dbCon.GetDbSet<Compositions>().ToList();
            listOfInstrumInCompositions = dbCon.GetDbSet<InstrumInCompositions>().ToList();
            listOfMessage = dbCon.GetDbSet<Message>().ToList();
            listOfPlaces = dbCon.GetDbSet<Places>().ToList();
            listOfPlayers = dbCon.GetDbSet<Players>().ToList();
            listOfInstrumentsPerPlayer = dbCon.GetDbSet<InstumentPerPlayer>().ToList();
            listOfPlayersInAppearances = dbCon.GetDbSet<PlayersInAppearances>().ToList();
        }
        #region function
        #region showing
        
        public List<Clients> GetAllClients()
        {
            return listOfClients;
        }
        public List<Compositions> GetAllCompositions()
        {
            return listOfCompositions;
        }
        public List<Instruments> GetAllInstruments()
        {
            return listOfInstruments;
        }
        public List<InstrumInCompositions> GetAllInstrumInCompositions()
        {
            return listOfInstrumInCompositions;
        }
        public List<Message> GetAllMessage()
        {
            return listOfMessage;
        }
        public List<Places> GetAllPlaces()
        {
            return listOfPlaces;
        }
        public List<Players> GetAllPlayers()
        {
            return listOfPlayers;
        }
        public List<PlayersInAppearances> GetAllPlayersInAppearances()
        {
            return listOfPlayersInAppearances;
        }
        public List<InstumentPerPlayer> GetAllInstrumentsPerPlayers()
        {
            return listOfInstrumentsPerPlayer;
        }

        #endregion
        #region insert
        //הוספת הופעה
        public string InsertAppearance(string style, DateTime dateA, int idCli, int codePlace, string pelPlace, DateTime startH, DateTime endH, List<Players> listPlayersInComp, int cost, List<Instruments> listOfInst)
        {
            if (itsOK == 0)
            {
                try
                {
                    InsertComposition(style, listPlayersInComp.Count, listOfInst);
                    dbCon.Execute<Appearances>(new Appearances() { codeA = listOfAppearances.Max(i => i.codeA) + 1, dateA = dateA, codePlays = codePlace, codeCli = idCli, pelPlays = pelPlace, startHour = startH.TimeOfDay, endHour = endH.TimeOfDay, codeComp = listOfCompositions.Max(c => c.codeComp), codeConductor = listPlayersInComp.FirstOrDefault(p => p.status == "activeC").codeP, cost = cost }, DBConection.ExecuteActions.Insert);
                    listOfAppearances = dbCon.GetDbSet<Appearances>().ToList();
                    foreach (var item in listPlayersInComp)
                    {
                        InsertPlayerInApp(listOfAppearances.Max(a => a.codeA), item.codeP);
                        //פעולת שליחה של אימייל לכל נגן
                    }
                    return "true";
                }
                catch
                {
                    return "false";
                }
            }
            return "the data contain errors";
        }
        //הוספת לקוח
        public string InsertClient(int id, string name, string phon1, string phon2, string email, int points)
        {
            if (itsOK == 0)
            {
                if (listOfClients.First(c => c.idC == id) == null)
                    try
                    {
                        dbCon.Execute<Clients>(new Clients() { codeCli = listOfClients.Max(c => c.codeCli), idC = id, fullNameC = name, pel1 = phon1, pel2 = phon2, email = email, points = points, status = "true" }, DBConection.ExecuteActions.Insert);
                        listOfClients = dbCon.GetDbSet<Clients>().ToList();
                        return "true";
                    }
                    catch
                    {
                        return "false";
                    }
                else
                {
                    return "existing client with same id";
                }
            }
            return "the data contain errors";
        }
        //הוספת הרכב
        public bool InsertComposition(string type, int numOfP, List<Instruments> instruments)
        {
            try
            {
                dbCon.Execute<Compositions>(new Compositions() { codeComp = listOfCompositions.Max(i => i.codeComp) + 1, type = type, numOfPlayers = numOfP }, DBConection.ExecuteActions.Insert);
                listOfCompositions = dbCon.GetDbSet<Compositions>().ToList();
                foreach (var item in instruments)
                {
                    InsertInstrumentInComp(item.codeInst, listOfCompositions.Max(i => i.codeComp));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //הוספת כלי
        public string InsertInstrument(string name, List<int> style, int size, int voice, int type)
        {
            if (listOfInstruments.First(c => c.nameInst == name) == null)
                try
                {
                    dbCon.Execute<Instruments>(new Instruments() { codeInst = listOfInstruments.Max(i => i.codeInst) + 1, nameInst = name, size = size, style = GetStyleOfInst(style), voice = voice, type = type }, DBConection.ExecuteActions.Insert);
                    listOfInstruments = dbCon.GetDbSet<Instruments>().ToList();
                    return "true";
                }
                catch
                {
                    return "false";
                }
            else
                return "existing instrument with same name";
        }
        // get the style of instrum by format in the DB
        public int GetStyleOfInst(List<int> style)
        {
            int res = 0; ;
            for (int i = 0; i < style.Count; i++)
            {
                if (i != 0)
                    res = res * 10 + style[i];
                else
                    res = res + style[i];

            }
            return res;
        }
        //הוספת כלי בהרכב
        public bool InsertInstrumentInComp(int codeI, int codeC)
        {
            try
            {
                dbCon.Execute<InstrumInCompositions>(new InstrumInCompositions() { codeIInComp = listOfInstrumInCompositions.Max(i => i.codeIInComp) + 1, codeInst = codeI, codeComp = codeC }, DBConection.ExecuteActions.Insert);
                listOfInstrumInCompositions = dbCon.GetDbSet<InstrumInCompositions>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //הוספת הודעה
        public bool InsertMessege(string text, int isImport, string source, string destination, string date, string status)
        {
            try
            {
                dbCon.Execute<Message>(new Message() { codeM = listOfMessage.Max(i => i.codeM) + 1, taxt = text, isImport = isImport, sourceM = source, destinationM = destination, DateSending = DateTime.Parse(date), status = status }, DBConection.ExecuteActions.Insert);
                listOfMessage = dbCon.GetDbSet<Message>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //הוספת מיקום
        public string InsertPlace(string city, string addres)
        {
            try
            {
                double locationX = 0, locationY = 0;
                //מציאת מיקום עי גוגל מאפס
                if (listOfPlaces.Where(p => p.locationX == locationX && p.locationY == locationY).ToList().Count() == 0)
                {
                    dbCon.Execute<Places>(new Places() { codeP = listOfPlaces.Max(i => i.codeP) + 1, city = city, addres = addres, locationX = locationX, locationY = locationY }, DBConection.ExecuteActions.Insert);
                    listOfPlaces = dbCon.GetDbSet<Places>().ToList();
                    return "true";
                }
                return "not found";
            }
            catch
            {
                return "false";
            }
        }
        //הוספת נגן
        public string InsertPlayer(Players player,Instruments[] instument)
        {
            if (itsOK == 0)
            {
                if (listOfPlayers.First(p => p.idP == player.idP) == null)
                    try
                    {
                        
                        dbCon.Execute<Players>(new Players() { codeP = listOfPlayers.Max(c => c.codeP)+1, idP = player.idP, fullNameP = player.fullNameP, pel = player.pel, email = player.email, codePlays = player.codePlays, price = player.price,daysWork=player.daysWork, status = "true" }, DBConection.ExecuteActions.Insert);
                        listOfPlayers = dbCon.GetDbSet<Players>().ToList();
                        foreach(var inst in instument)
                        {
                            InsertInstrumPerPlayer(inst,int.Parse(player.idP.ToString()));
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
            return "the data contain errors";
        }
        
        //מחיקת כלי של נגן
        //עדכון כלי של נגן
        //הוספת כלי לנגן
        private void InsertInstrumPerPlayer(Instruments inst,int idPlayer)
        {
            throw new NotImplementedException();
        }

        //הוספת נגן בהופעה
        public bool InsertPlayerInApp(int codeA, int codeP)
        {
            try
            {
                dbCon.Execute<PlayersInAppearances>(new PlayersInAppearances() { codePInA = listOfPlayersInAppearances.Max(i => i.codePInA) + 1, codeA = codeA, codeP = codeP }, DBConection.ExecuteActions.Insert);
                listOfPlayersInAppearances = dbCon.GetDbSet<PlayersInAppearances>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region delete
        //מחיקת הופעה
        public bool DeleteAppearance(int codeA)
        {
            try
            {
                if (DateTime.Parse(listOfAppearances.First(a => a.codeA == codeA).dateA.ToString()).Day >= DateTime.Now.Day + 14)
                {
                    //מחיקת הנגנים מהופעה
                    foreach (var item in listOfPlayersInAppearances)
                    {
                        //שלחית הודעת אימייל אודות ביטול ההופעה
                        if (item.codeA == codeA)
                            DeletePlayerInApp(item.codePInA);
                    }
                    //מחיקת ההרכב של ההופעה
                    DeleteComposition(listOfAppearances.First(a => a.codeA == codeA).codeComp);
                    dbCon.Execute<Appearances>(listOfAppearances.First(c => c.codeA == codeA), DBConection.ExecuteActions.Delete);
                    listOfAppearances = dbCon.GetDbSet<Appearances>().ToList();
                    return true;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        //מחיקת לקוח
        public bool DeleteClient(int codeC)
        {
            try
            {
                dbCon.Execute<Clients>(listOfClients.First(c => c.idC == codeC), DBConection.ExecuteActions.Delete);
                listOfClients = dbCon.GetDbSet<Clients>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //מחיקת הרכב
        public bool DeleteComposition(int codeC)
        {
            try
            {
                if (listOfCompositions.First(c => c.codeComp == codeC) != null)
                {
                    foreach (var item in listOfInstrumInCompositions)
                    {
                        if (item.codeComp == codeC)
                            DeleteInstrumentInComp(item.codeIInComp);
                    }
                    dbCon.Execute<Compositions>(listOfCompositions.First(c => c.codeComp == codeC), DBConection.ExecuteActions.Delete);
                    listOfCompositions = dbCon.GetDbSet<Compositions>().ToList();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //מחיקת כלי בהרכב
        public bool DeleteInstrumentInComp(int codeIinC)
        {
            try
            {
                dbCon.Execute<InstrumInCompositions>(listOfInstrumInCompositions.First(i => i.codeIInComp == codeIinC), DBConection.ExecuteActions.Delete);
                listOfInstrumInCompositions = dbCon.GetDbSet<InstrumInCompositions>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //מחיקת הודעה
        public bool DeleteMessege(int codeM)
        {
            try
            {
                dbCon.Execute<Message>(listOfMessage.First(m => m.codeM == codeM), DBConection.ExecuteActions.Delete);
                listOfMessage = dbCon.GetDbSet<Message>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //מחיקת מיקום
        public bool DeletePlace(int codeP)
        {
            try
            {
                dbCon.Execute<Places>(listOfPlaces.First(p => p.codeP == codeP), DBConection.ExecuteActions.Delete);
                listOfPlaces = dbCon.GetDbSet<Places>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //מחיקת נגן
        public bool DeletePlayer(int codeP)
        {
            try
            {
                if (listOfPlayers.First(p => p.idP == codeP) != null)
                {
                    if (listOfPlayersInAppearances.First(p => p.codeP == codeP) != null)
                    {
                        //מציאת מחליף לנגן בכל ההופעות שהוא משובץ בהן
                        ChangeDeletedPlayerInHisAppearances(codeP);
                    }
                    //מחיקת הכלים שלו
                    foreach (var item in listOfInstrumentsPerPlayer.Where(p=>p.codeP==codeP))
                    {
                        DeleteInstrumPerPlayer(item.codeIPerP);
                    }
                    dbCon.Execute<Players>(listOfPlayers.First(p => p.idP == codeP), DBConection.ExecuteActions.Delete);
                    listOfPlayers = dbCon.GetDbSet<Players>().ToList();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        //מחיקת כלי פר נגן
        private bool DeleteInstrumPerPlayer(int codeIPerP)
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
        #endregion
        #region updete
        //עדכון הופעה
        public bool UpDateAppearance(Appearances appearanceToUpdate)
        {
            try
            {
                dbCon.Execute<Appearances>(appearanceToUpdate, DBConection.ExecuteActions.Update);
                listOfAppearances = dbCon.GetDbSet<Appearances>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //עדכון לקוח
        public bool UpDateClient(Clients client)
        {
            try
            {
                dbCon.Execute<Clients>(client, DBConection.ExecuteActions.Update);
                listOfAppearances = dbCon.GetDbSet<Appearances>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        //עדכון נגן
        public bool UpDatePlayer(Players player)
        {
            try
            {
                dbCon.Execute<Players>(player, DBConection.ExecuteActions.Update);
                listOfPlayers = dbCon.GetDbSet<Players>().ToList();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        //יצירת הרכב נגנים להופעה
        public Dictionary<int, List<Instruments>> CreateComposition(int style, int sizePlace)
        {
            //dictionary of 4 options to compositions
            Dictionary<int, List<Instruments>> dictOfComps = new Dictionary<int, List<Instruments>>();
            //list of cord instruments high
            List<Instruments> highCordInst = listOfInstruments.Where(i => i.voice == 1 && FindStyle(style, i.style) && i.type == 0).ToList();
            //list of cord instruments low
            List<Instruments> lowCordInst = listOfInstruments.Where(i => i.voice == 0 && FindStyle(style, i.style) && i.type == 0).ToList();
            //list of exhaling instruments high
            List<Instruments> highExhalingInst = listOfInstruments.Where(i => i.voice == 1 && FindStyle(style, i.style) && i.type == 1).ToList();
            //list of exhaling instruments low
            List<Instruments> lowExhalingInst = listOfInstruments.Where(i => i.voice == 0 && FindStyle(style, i.style) && i.type == 1).ToList();
            //list of clicking instruments 
            List<Instruments> ClickingInst = listOfInstruments.Where(i => i.voice == 0 && FindStyle(style, i.style) && i.type == 2).ToList();
            //list of mix clicking instruments 
            List<Instruments> ClickingForMixInst = listOfInstruments.Where(i => i.voice == 1 && FindStyle(style, i.style) && i.type == 2).ToList();
            //mat to calculation number of instrumentm from all of kind
            int[,] matPercents = new int[6, 7] { { 60, 83, 17, 35, 85, 15, 5 }, { 70, 97, 3, 10, 100, 0, 20 }, { 60, 67, 33, 30, 67, 33, 10 }, { 75, 87, 13, 15, 100, 0, 10 }, { 50, 50, 50, 46, 43, 57, 4 }, { 60, 50, 50, 30, 67, 33, 10 } };
            //number of instrum in the composition
            int numOfInstumentsInTheComp = ((sizePlace - 15) / 4) + 1;
            //number of instrum per type
            //0=high cord,1=low cord,2=high exhaling,3=low exhaling,4= clicking
            double[] numOfInstOfType = new double[5];
            //local index
            double numInst;
            if (numOfInstumentsInTheComp < 100)
            {
                numInst = numOfInstumentsInTheComp * (matPercents[style - 1, 0] * 0.01);
                numOfInstOfType[0] = numInst * (matPercents[style - 1, 1] * 0.01);
                numOfInstOfType[1] = numInst * (matPercents[style - 1, 2] * 0.01);
                numInst = numOfInstumentsInTheComp * (matPercents[style - 1, 3] * 0.01);
                numOfInstOfType[2] = numInst * (matPercents[style - 1, 4] * 0.01);
                numOfInstOfType[3] = numInst * (matPercents[style - 1, 5] * 0.01);
                numOfInstOfType[4] = numOfInstumentsInTheComp * (matPercents[style - 1, 6] * 0.01);
            }
            else
            {
                numInst = numOfInstumentsInTheComp * ((matPercents[style - 1, 0] - 10) * 0.01);
                numOfInstOfType[0] = numInst * (matPercents[style - 1, 1] * 0.01);
                numOfInstOfType[1] = numInst * (matPercents[style - 1, 2] * 0.01);
                numInst = numOfInstumentsInTheComp * ((matPercents[style - 1, 3] + 10) * 0.01);
                numOfInstOfType[2] = numInst * (matPercents[style - 1, 4] * 0.01);
                numOfInstOfType[3] = numInst * (matPercents[style - 1, 5] * 0.01);
                numOfInstOfType[4] = numOfInstumentsInTheComp * (matPercents[style - 1, 6] * 0.01);
            }
            for (int i = 0; i < 4; i++)
            {
                if (i > 0)
                {
                    highCordInst = listOfInstruments.Where(inst => inst.voice == 1 && FindStyle(style, inst.style) && inst.type == 0).ToList();
                    lowCordInst = listOfInstruments.Where(inst => inst.voice == 0 && FindStyle(style, inst.style) && inst.type == 0).ToList();
                    highExhalingInst = listOfInstruments.Where(inst => inst.voice == 1 && FindStyle(style, inst.style) && inst.type == 1).ToList();
                    lowExhalingInst = listOfInstruments.Where(inst => inst.voice == 0 && FindStyle(style, inst.style) && inst.type == 1).ToList();
                    ClickingInst = listOfInstruments.Where(inst => inst.voice == 0 && FindStyle(style, inst.style) && inst.type == 2).ToList();
                    ClickingForMixInst = listOfInstruments.Where(inst => inst.voice == 1 && FindStyle(style, inst.style) && inst.type == 2).ToList();
                }
                sizeOfPlace = sizePlace;
                dictOfComps[i] = new List<Instruments>();
                if ((i == 0 || i == 1)/*&&style!=6*/)
                    dictOfComps[i].Add(listOfInstruments.First(a => a.nameInst == "פסנתר"));
                else
                    dictOfComps[i].Add(listOfInstruments.First(a => a.nameInst == "אורגן"));
                sizeOfPlace -= dictOfComps[i][0].size;
                dictOfComps[i].Add(ClickingForMixInst.First(a => a.nameInst == "תופים קלאס"));
                ClickingForMixInst.Remove(ClickingForMixInst.First(a => a.nameInst == "תופים קלאס"));
                sizeOfPlace -= dictOfComps[i][1].size;
                dictOfComps[i].Add(lowCordInst.First(a => a.nameInst == "גיטרה בס"));
                lowCordInst.Remove(lowCordInst.First(a => a.nameInst == "גיטרה בס"));
                sizeOfPlace -= dictOfComps[i][2].size;
                if (numOfInstOfType[4] >= 1)
                {
                    dictOfComps[i] = MixInsrumentsRandom(ClickingForMixInst, dictOfComps[i]);
                    sizeOfPlace -= 6;
                }
                //random of high cord instruments
                dictOfComps[i] = InsrumentsRandom(numOfInstOfType[0], highCordInst, dictOfComps[i]);
                //random of low cord instruments
                dictOfComps[i] = InsrumentsRandom(numOfInstOfType[1] - 1, lowCordInst, dictOfComps[i]);
                //random of high exhaling instruments
                dictOfComps[i] = InsrumentsRandom(numOfInstOfType[2], highExhalingInst, dictOfComps[i]);
                //random of low exhaling instruments
                dictOfComps[i] = InsrumentsRandom(numOfInstOfType[3], lowExhalingInst, dictOfComps[i]);
                //random of clicking instruments
                dictOfComps[i] = InsrumentsRandom(numOfInstOfType[4] - 1, ClickingInst, dictOfComps[i]);
            }
            return dictOfComps;

        }
        //הגרלת מיקס תופים
        private List<Instruments> MixInsrumentsRandom(List<Instruments> listInst, List<Instruments> listInstFomDict)
        {
            for (int i = 0; i < 5; i++)
            {
                if (listInst.Count == 0)
                    break;
                listInstFomDict.Add(listInst[random.Next(listInst.Count)]);
                listInst.Remove(listInst.First(a => a.codeInst == listInstFomDict[listInstFomDict.Count - 1].codeInst));
            }
            return listInstFomDict;
        }
        //return true if the style in the list of styles
        public bool FindStyle(int style, int styleOfInst)
        {
            while (styleOfInst != 0)
            {
                if (styleOfInst % 10 == style)
                    return true;
                styleOfInst /= 10;
            }
            return false;
        }
        //הגרלת כלים לפי סוג
        public List<Instruments> InsrumentsRandom(double numOfInst,List<Instruments> listInst, List<Instruments> listInstFomDict)
        {
            //if(numOfInst%1>0.5)
            // numOfInst = numOfInst - (numOfInst % 1)+1;
            //else
            numOfInst = numOfInst - (numOfInst % 1);
            while (numOfInst > 0)
            {
                if (sizeOfPlace < 4)
                    break;
                listInstFomDict.Add(listInst[random.Next(listInst.Count)]);
                sizeOfPlace -= listInstFomDict[listInstFomDict.Count - 1].size;
                numOfInst -= 1;
                if (listInstFomDict[listInstFomDict.Count - 1].nameInst == "נבל")
                    listInst.Remove(listOfInstruments.First(a => a.nameInst == "נבל"));
                else
                    if (listInstFomDict[listInstFomDict.Count - 1].nameInst == "עוגב")
                    listInst.Remove(listOfInstruments.First(a => a.nameInst == "עוגב"));
                else
                        if (listInstFomDict[listInstFomDict.Count - 1].nameInst == "אקורדיון")
                    listInst.Remove(listOfInstruments.First(a => a.nameInst == "אקורדיון"));
                if (sizeOfPlace < 0)
                {
                    sizeOfPlace += listInstFomDict[listInstFomDict.Count - 1].size;
                    listInstFomDict.Remove(listInstFomDict[listInstFomDict.Count - 1]);
                    numOfInst += 1;
                }
            }
            return listInstFomDict;
        }
        //הצגת יומן אירועים לפי קוד נגן
        public List<Appearances> ScheduleForPlayer(int codePlayer)
        {
            List<PlayersInAppearances> appOfPlayer = listOfPlayersInAppearances.Where(a => a.codeP == codePlayer).ToList();
            List<Appearances> appearances = new List<Appearances>();
            foreach (var item in appOfPlayer)
            {
                appearances.Add(listOfAppearances.First(a => a.codeA == item.codeA));
            }
            return appearances;
        }
        //הצגת הודעות מנהל לפי קוד נגן
        public List<Message> MessegesForPlayer(int codePlayer)
        {
            List<Message> messagesOfPlayer = listOfMessage.Where(m=>m.destinationM==codePlayer.ToString()|| m.destinationM =="all player").ToList();
            return messagesOfPlayer;
        }
        //בחירת נגנים להרכב מסויים על פי כלי , זמינות, רייטינג ומחיר
        public int PlayerForAppearance(int codeComp)
        {
            return 3;
        }
        //החלפת הנגן הנתון כקלט בכל ההופעות בהן הוא משובץ
        //לא סיימתי
        private void ChangeDeletedPlayerInHisAppearances(int codeP)
        {
            foreach (var item in listOfPlayersInAppearances)
            {
                if (item.codeP == codeP)
                {
                    int codePChange = 0;
                    //מצא מחליף
                    //
                    InsertPlayerInApp(item.codeA, codePChange);
                    DeletePlayerInApp(item.codePInA);
                }
            }
        }
        //פעולה בשביל התשלום##
        //שליחת הודעת אימייל לנגנים(לזמן מהוספת ומחיקת הופעה)##
        //הצגת הכלים לפי קוד הרכב
        public List<Instruments> InstrumentsOfComp(int codeComp)
        {
            List<InstrumInCompositions> instOfComp = listOfInstrumInCompositions.Where(a => a.codeComp == codeComp).ToList();
            List<Instruments> instruments = new List<Instruments>();
            foreach (var item in instOfComp)
            {
                instruments.Add(listOfInstruments.First(i => i.codeInst == item.codeInst));
            }
            return instruments;
        }
        #endregion
    }
}
