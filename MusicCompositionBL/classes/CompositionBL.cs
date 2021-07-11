using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCompositionDAL;
using Models;
namespace MusicCompositionBL.classes
{

    public class CompositionBL
    {
        DBConection dbCon;
        public List<CompositionsModel> listOfCompositions;
        InstrumentsBL instrumentsBL = new InstrumentsBL();
        InstrumOfCompBL instrumOfCompBL = new InstrumOfCompBL();
        public CompositionBL()
        {
            dbCon = new DBConection();
            listOfCompositions =ConvertListToModel( dbCon.GetDbSet<Compositions>().ToList());
        }
        public List<CompositionsModel> GetAllCompositions()
        {
            return listOfCompositions;
        }
        //הוספת הרכב
        public int InsertComposition(MusicCompositionBL.classes.addCompTApp a)
        {
            try
            {
                dbCon.Execute<Compositions>(new Compositions() { codeComp =ConvertListToEF(listOfCompositions).Max(i => i.codeComp) +1, type = a.style, numOfPlayers = a.numOfP }, DBConection.ExecuteActions.Insert);
                listOfCompositions =ConvertListToModel( dbCon.GetDbSet<Compositions>().ToList());
                foreach (var item in a.listOfInst)
                {
                    instrumOfCompBL.InsertInstrumentInComp(item.codeInst, listOfCompositions.Max(i => i.codeComp));
                }
                return listOfCompositions.Max(i => i.codeComp);
            }
            catch
            {
                return 0;
            }
        }
        //מחיקת הרכב
        public int DeleteComposition(CompositionsModel composition)
        {
            try
            {
                if (listOfCompositions.First(c => c.codeComp == composition.codeComp) !=null)
                {
                    foreach (var item in instrumOfCompBL.listOfInstrumInCompositions)
                    {
                        if (item.codeComp == composition.codeComp)
                            instrumOfCompBL.DeleteInstrumentInComp(item.codeIInComp);
                    }
                    dbCon.Execute<CompositionsModel>(listOfCompositions.First(c => c.codeComp == composition.codeComp), DBConection.ExecuteActions.Delete);
                    listOfCompositions = dbCon.GetDbSet<CompositionsModel>().ToList();
                    return 1;
                }
                return 2;
            }
            catch
            {
                return 0;
            }
        }
        //הצגת הכלים לפי קוד הרכב
        public List<InstrumentsModel> InstrumentsOfComp(int codeComp)
        {
            List<InstrumInCompositions> instOfComp = instrumOfCompBL.listOfInstrumInCompositions.Where(a => a.codeComp == codeComp).ToList();
            List<InstrumentsModel> instruments = new List<InstrumentsModel>();
            foreach (var item in instOfComp)
            {
                instruments.Add(instrumentsBL.listOfInstruments.First(i => i.codeInst == item.codeInst));
            }
            return instruments;
        }
        public Compositions ConvertCompositionsToEF(CompositionsModel c)
        {
            return new Compositions
            {
                codeComp = c.codeComp,
                type = c.type,
                numOfPlayers = c.numOfPlayers

            };
        }
        public static CompositionsModel ConvertCompositionsToModel(Compositions c)
        {
            return new CompositionsModel
            {
                codeComp=c.codeComp,type=c.type,numOfPlayers=c.numOfPlayers

            };
        }
        public static List<CompositionsModel> ConvertListToModel(List<Compositions> i)
        {
            List<CompositionsModel> l = new List<CompositionsModel>();
            Compositions inst = new Compositions();
            foreach (var item in i)
            {
                inst.codeComp = item.codeComp;
                inst.type = item.type;
                inst.numOfPlayers = item.numOfPlayers;
                l.Add(ConvertCompositionsToModel(inst));
            }
            return l;
        }
        public static List<Compositions> ConvertListToEF(List<CompositionsModel> i)
        {
            List<Compositions> l = new List<Compositions>();
            Compositions inst = new Compositions();
            foreach (var item in i)
            {
                inst.codeComp = item.codeComp;
                inst.type = item.type;
                inst.numOfPlayers = item.numOfPlayers;
                l.Add((inst));
            }
            return l;
        }
    }
}
