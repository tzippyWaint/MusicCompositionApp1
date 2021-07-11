using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCompositionDAL;
using Models;
namespace MusicCompositionBL.classes
{
    public class InstrumentsBL
    {
        //dictionary of 4 options to compositions
        Dictionary<int, List<InstrumentsModel>> dictOfComps = new Dictionary<int, List<InstrumentsModel>>();
        DBConection dbCon;
        public List<InstrumentsModel> listOfInstruments;
        public List<Instruments> list;
        int sizeOfPlace;
        Random random = new Random();
        public InstrumentsBL()
        {
            dbCon = new DBConection();
            list = dbCon.GetDbSet<Instruments>().ToList();
            listOfInstruments= ConvertListToModel(list);
        }
        public List<InstrumentsModel> GetAllInstruments()
        {
            return listOfInstruments;
        }
        //הוספת כלי
        public string InsertInstrument(string name, int style, int size, int voice, int type)
        {
            if (listOfInstruments.Find(c => c.nameInst == name) == null)
                try
                {
                    dbCon.Execute<Instruments>(new Instruments() { codeInst = listOfInstruments.Max(i => i.codeInst) + 1, nameInst = name, size = size, style = (style), voice = voice, type = type }, DBConection.ExecuteActions.Insert);
                    listOfInstruments = ConvertListToModel( dbCon.GetDbSet<Instruments>().ToList());
                    return "true";
                }
                catch
                {
                    return "false";
                }
            else
                return "same";
        }
        //get the style of instrum by format in the DB
        //public int GetStyleOfInst(List<int> style)
        //{
        //    int res = 0; ;
        //    for (int i = 0; i < style.Count; i++)
        //    {
        //        if (i != 0)
        //            res = res * 10 + style[i];
        //        else
        //            res = res + style[i];

        //    }
        //    return res;
        //}

        //יצירת הרכב כלים להופעה
        public Dictionary<int, List<InstrumentsModel>> CreateComposition(int style, int sizePlace)
        {
            //list of cord instruments high
            List<InstrumentsModel> highCordInst = listOfInstruments.Where(i => i.voice == 1 && FindStyle(style, i.style) && i.type == 0).ToList();
            //list of cord instruments low
            List<InstrumentsModel> lowCordInst = listOfInstruments.Where(i => i.voice == 0 && FindStyle(style, i.style) && i.type == 0).ToList();
            //list of exhaling instruments high
            List<InstrumentsModel> highExhalingInst = listOfInstruments.Where(i => i.voice == 1 && FindStyle(style, i.style) && i.type == 1).ToList();
            //list of exhaling instruments low
            List<InstrumentsModel> lowExhalingInst = listOfInstruments.Where(i => i.voice == 0 && FindStyle(style, i.style) && i.type == 1).ToList();
            //list of clicking instruments 
            List<InstrumentsModel> ClickingInst = listOfInstruments.Where(i => i.voice == 0 && FindStyle(style, i.style) && i.type == 2).ToList();
            //list of mix clicking instruments 
            List<InstrumentsModel> ClickingForMixInst = listOfInstruments.Where(i => i.voice == 1 && FindStyle(style, i.style) && i.type == 2).ToList();
            //mat to calculation number of instrumentm from all of kind
            int[,] matPercents = new int[6, 7] { { 60, 83, 17, 35, 85, 15, 5 }, { 70, 97, 3, 10, 100, 0, 20 }, { 60, 67, 33, 30, 67, 33, 10 }, 
                { 75, 87, 13, 15, 100, 0, 10 }, { 50, 50, 50, 46, 43, 57, 4 }, { 60, 50, 50, 30, 67, 33, 10 } };
            //number of instrum in the composition
            if (sizePlace > 400)
                sizePlace = 400;
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
                dictOfComps[i] = new List<InstrumentsModel>();
                if ((i == 0 || i == 1))
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
                dictOfComps[i].Add(listOfInstruments.First(a => a.nameInst == "מנצח"));
                if (numOfInstOfType[4] >= 1)
                {
                    dictOfComps[i] = MixInsrumentsRandom(ClickingForMixInst, dictOfComps[i]);
                    sizeOfPlace -= 6;
                }
                //random of high cord instruments
                InsrumentsRandom(numOfInstOfType[0], highCordInst, i);
                //random of low cord instruments
                InsrumentsRandom(numOfInstOfType[1] - 1, lowCordInst, i);
                //random of high exhaling instruments
                InsrumentsRandom(numOfInstOfType[2], highExhalingInst, i);
                //random of low exhaling instruments
                InsrumentsRandom(numOfInstOfType[3], lowExhalingInst, i);
                //random of clicking instruments
                InsrumentsRandom(numOfInstOfType[4] - 1, ClickingInst, i);
               
            }
            return dictOfComps;

        }
        //הגרלת מיקס תופים
        private List<InstrumentsModel> MixInsrumentsRandom(List<InstrumentsModel> listInst, List<InstrumentsModel> listInstFomDict)
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
        public void InsrumentsRandom(double numOfInst, List<InstrumentsModel> listInst, int indexe)
        {
            numOfInst = numOfInst - (numOfInst % 1);
            while (numOfInst > 0)
            {
                if (sizeOfPlace < 4)
                    break;
                dictOfComps[indexe].Add(listInst[random.Next(listInst.Count)]);
                sizeOfPlace -= dictOfComps[indexe][dictOfComps[indexe].Count - 1].size;
                numOfInst -= 1;
                if (dictOfComps[indexe][dictOfComps[indexe].Count - 1].nameInst == "נבל")
                    listInst.Remove(listOfInstruments.First(a => a.nameInst == "נבל"));
                else
                    if (dictOfComps[indexe][dictOfComps[indexe].Count - 1].nameInst == "עוגב")
                    listInst.Remove(listOfInstruments.First(a => a.nameInst == "עוגב"));
                else
                        if (dictOfComps[indexe][dictOfComps[indexe].Count - 1].nameInst == "אקורדיון")
                    listInst.Remove(listOfInstruments.First(a => a.nameInst == "אקורדיון"));
                if (sizeOfPlace < 0)
                {
                    sizeOfPlace += dictOfComps[indexe][dictOfComps[indexe].Count - 1].size;
                    dictOfComps[indexe].Remove(dictOfComps[indexe][dictOfComps[indexe].Count - 1]);
                    numOfInst += 1;
                }
            }
        }
        
        public static InstrumentsModel ConvertInstrumentsToModel(Instruments i)
        {
            return new InstrumentsModel
            {
                codeInst=i.codeInst,nameInst=i.nameInst,size=i.size,style=i.style,voice=i.voice,type=i.type
            };
        }
        public static List<InstrumentsModel> ConvertListToModel(List<Instruments> i)
        {
            List<InstrumentsModel> l = new List<InstrumentsModel>();
            Instruments inst = new Instruments();
            foreach (var item in i)
            {
                inst.codeInst = item.codeInst;
                inst.nameInst = item.nameInst;
                inst.size = item.size;
                inst.style = item.style;
                inst.voice = item.voice;
                inst.type = item.type;
                l.Add(ConvertInstrumentsToModel(inst));
            }
            return l;
        }
    }
}









