using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MusicCompositionBL
{
    class Validation
    {
        //וולידצית מספרים
        Regex regexNumbers = new Regex("[0-9]");
        //ולידצית אותיות
        Regex regexLetters = new Regex("[a-z_A-Z_א-ת]");
        //ולידצית פלאפון
        Regex regexphon = new Regex("^[05]");
        //האם הקלט מורכב ממספרים בלבד
        public bool IsNumbers(string strToCheck)
        {
            if (regexNumbers.IsMatch(strToCheck))
                return true;
            return false;
        }
        //האם הקלט מורכב מאותיות בלבד
        public bool IsLetters(string strToCheck)
        {
            if (regexLetters.IsMatch(strToCheck))
                return true;
            return false;
        }
        //האם המספר זהות תקין
        public bool IsId(string strToCheck)
        {
            int id;
            if (regexLetters.IsMatch(strToCheck))
                if ((int.TryParse(strToCheck, out id)))
                    if (checkId(id))
                        return true;
            return false;
        }
        //בדיקת תקינות לתז
        private bool checkId(int id)
        {
            int sBikoret = id % 10, sifraNocecit = 0, sum = 0, index = 2;
            id /= 10;
            while (id != 0)
            {
                sifraNocecit = (id % 10) * index;
                if (sifraNocecit > 9)
                {
                    sifraNocecit = sifraNocecit % 10 + sifraNocecit / 10;
                }
                sum += sifraNocecit;
                id /= 10;
                index = (index == 2) ? 1 : 2;
            }
            if ((sum == 30 && sBikoret == 0) || (30 - sum == sBikoret))
                return true;
            return false;
        }
        //האם הכתובת המייל תקינה
        //לבדוק האם זה מספיק
        public bool IsEmail(string strToCheck)
        {
            if ((strToCheck).Contains("@") || strToCheck.Contains(".co.il"))
                return true;
            return false;
        }
        //האם מספר הפלאפון תקין
        public bool IsPhonNumber(string strToCheck)
        {
            if (regexNumbers.IsMatch(strToCheck) && strToCheck.Length == 10 && regexphon.IsMatch(strToCheck))
                return true;
            return false;
        }
    }
}
