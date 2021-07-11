using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicCompositionDAL;
using Models;
namespace MusicCompositionBL.classes
{

    public class InstrumOfCompBL
    {
        DBConection dbCon;
        public List<InstrumInCompositions> listOfInstrumInCompositions;
        public InstrumOfCompBL()
        {
            dbCon = new DBConection();
            listOfInstrumInCompositions = dbCon.GetDbSet<InstrumInCompositions>().ToList();
        }
        public List<InstrumInCompositions> GetAllInstrumInCompositions()
        {
            return listOfInstrumInCompositions;
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
        public Instruments ConvertInstrumentsToEF(Instruments i)
        {
            return new Instruments
            {
                codeInst = i.codeInst,
                nameInst = i.nameInst,
                size = i.size,
                style = i.style,
                voice = i.voice,
                type = i.type
            };
        }
        public static InstumentInCompModel ConvertInstrumInCompositionsToModel(InstrumInCompositions i)
        {
            return new InstumentInCompModel
            {
                codeInst = i.codeInst,
                codeComp = i.codeComp,
                codeIInComp = i.codeIInComp
            };
        }
    }
}
